using AutoMapper;
using Fintech.Application.DTOs.Aml;
using Fintech.Application.Interfaces.Aml;
using Fintech.Domain.Entities;
using Fintech.Domain.Interfaces;

namespace Fintech.Application.Services;

public class AmlService : IAmlService
{
    private readonly IAmlRepository _amlRepository;
    private readonly IGeminiAmlService _geminiAmlService;
    private readonly IMapper _mapper;

    public AmlService(IAmlRepository amlRepository, IGeminiAmlService geminiAmlService, IMapper mapper)
    {
        _amlRepository = amlRepository;
        _geminiAmlService = geminiAmlService;
        _mapper = mapper;
    }

    public async Task<AmlResultDto> CheckAsync(AmlRequestDto req, Guid authId)
    {
        (string riskLevel, string resultSummary) = await _geminiAmlService.AnalyzeAsync(req.FullName, req.DocumentNumber ?? "", req.Country);

        var amlCheck = new AmlCheck
        {
            Id = Guid.NewGuid(),
            AuthId = authId,
            FullName = req.FullName,
            DocumentNumber = req.DocumentNumber ?? "",
            Country = req.Country,
            RiskLevel = riskLevel,
            ResultSummary = resultSummary,
            CreatedAt = DateTime.UtcNow
        };

        var addedAmlCheck = await _amlRepository.AddAsync(amlCheck);
        return _mapper.Map<AmlResultDto>(addedAmlCheck);
    }

    public async Task<IEnumerable<AmlResultDto>> GetChecksByAuthIdAsync(Guid authId)
    {
        var amlChecks = await _amlRepository.GetAllByAuthIdsync(authId);
        return _mapper.Map<IEnumerable<AmlResultDto>>(amlChecks);
    }

    public async Task<AmlResultDto?> GetByIdAsync(Guid id)
    {
        var amlCheck = await _amlRepository.GetByIdAsync(id);
        return amlCheck != null ? _mapper.Map<AmlResultDto>(amlCheck) : null;
    }
}
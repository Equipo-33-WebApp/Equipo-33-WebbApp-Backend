using AutoMapper;
using Fintech.Application.DTOs.Aml;
using Fintech.Application.Interfaces.Aml;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;
using Fintech.Domain.Interfaces;

namespace Fintech.Application.Services;

public class AmlService : IAmlService
{
    private readonly IAmlRepository _amlRepository;
    private readonly IGeminiAmlService _geminiAmlService;
    private readonly IMapper _mapper;
    private readonly IPymeRepository _pymeRepository;

    public AmlService(IAmlRepository amlRepository, IGeminiAmlService geminiAmlService, IMapper mapper, IPymeRepository pymeRepository)
    {
        _amlRepository = amlRepository;
        _geminiAmlService = geminiAmlService;
        _mapper = mapper;
        _pymeRepository = pymeRepository;
    }

    public async Task<AmlResultDto> CheckAsync(Guid authId)
    {
        var pyme = await _pymeRepository.GetByAuthIdAsync(authId);
        if (pyme == null)
        {
            throw new Exception("No se encontró una PYME asociada al usuario autenticado.");
        }

        var geminiRequest = new GeminiAmlRequestDto
        {
            CompanyName = pyme.CompanyName,
            Address = pyme.Address,
            Sector = pyme.Sector,
            Employees = pyme.Employees,
            Phone = pyme.Phone
        };

        var geminiResponse = await _geminiAmlService.AnalyzeAsync(geminiRequest);

        var amlCheck = new AmlCheck
        {
            Id = Guid.NewGuid(),
            AuthId = authId,
            PymeId = pyme.Id,
            RiskLevel = geminiResponse.RiskLevel,
            ResultSummary = geminiResponse.Summary,
            Flags = geminiResponse.Flags.ToList(),
            RequiresManualReview = geminiResponse.RequiresManualReview,
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
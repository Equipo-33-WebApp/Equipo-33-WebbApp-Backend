using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;

namespace Fintech.Application.Services
{
    public class CreditFormService : ICreditFormService
    {
        private readonly ICreditFormRepository _creditFormRepository;
        private readonly IMapper _mapper;
        public CreditFormService(ICreditFormRepository creditFormRepository, IMapper mapper)
        {
            _creditFormRepository = creditFormRepository;
            _mapper = mapper;
        }
        public async Task<CreditForm?> GetByIdAsync(Guid id)
        {
            return await _creditFormRepository.GetByIdAsync(id);
        }
        public async Task<CreditForm?> GetByAuthIdAsync(Guid authId)
        {
            return await _creditFormRepository.GetByAuthIdAsync(authId);
        }
        public async Task<CreditForm?> GetDraftByAuthIdAsync(Guid authId)
        {
            return await _creditFormRepository.GetDraftByAuthIdAsync(authId);
        }
        public async Task<CreditForm> CreateAsync(CreateCreditFormDto dto, Guid authId)
        {
            var creditForm = new CreditForm
            {
                Id = Guid.NewGuid(),
                UserId = authId,
                PymeId = dto.PymeId,
                Amount = dto.Amount,
                TermInMonths = dto.TermInMonths,
                AnnualIncome = dto.AnnualIncome,
                NetIncome = dto.NetIncome,
                CreditDestination = dto.CreditDestination,
                RiskLevel = dto.RiskLevel,
                Purpose = dto.Purpose,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
            };

            return await _creditFormRepository.AddAsync(creditForm);
        }
        public async Task<CreditForm?> UpdateAsync(UpdateCreditFormDto dto, Guid authId)
        {
            var existingCreditForm = await _creditFormRepository.GetByAuthIdAsync(authId);
            if (existingCreditForm == null)
            {
                return null;
            }
            //existingCreditForm.PymeId = dto.PymeId;
            existingCreditForm.Amount = dto.Amount;
            existingCreditForm.TermInMonths = dto.TermInMonths;
            existingCreditForm.AnnualIncome = dto.AnnualIncome;
            existingCreditForm.NetIncome = dto.NetIncome;
            existingCreditForm.CreditDestination = dto.CreditDestination;
            existingCreditForm.RiskLevel = dto.RiskLevel;
            existingCreditForm.Status = dto.Status;
            existingCreditForm.Purpose = dto.Purpose;
            return await _creditFormRepository.UpdateAsync(existingCreditForm);

        }
    }
}

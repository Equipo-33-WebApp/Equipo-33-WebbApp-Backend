using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<CreditForm> CreateAsync(CreateCreditFormDto dto, Guid authId)
        {
            var creditForm = new CreditForm
            {
                Id = Guid.NewGuid(),
                UserId = authId,
                PymeId = dto.PymeId,
                Amount = dto.Amount,
                Purpose = dto.Purpose,
                Status = "pendiente",
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
            existingCreditForm.Purpose = dto.Purpose;
            return await _creditFormRepository.UpdateAsync(existingCreditForm);

        }
    }
}

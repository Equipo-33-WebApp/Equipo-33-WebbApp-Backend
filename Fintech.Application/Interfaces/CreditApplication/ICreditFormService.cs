using Fintech.Application.DTOs;
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.CreditApplication
{
    public interface ICreditFormService
    {
        Task<CreditForm?> GetByIdAsync(Guid id);
        Task<CreditForm?> GetByAuthIdAsync(Guid authId);
        Task<CreditForm> CreateAsync(CreateCreditFormDto dto, Guid authId);
        Task<CreditForm?> UpdateAsync(UpdateCreditFormDto dto,  Guid authId);
    }
}
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.CreditApplication
{
    public interface ICreditFormRepository
    {
        Task<CreditForm?> GetByIdAsync(Guid id);
        Task<CreditForm?> GetByAuthIdAsync(Guid authId);
        Task<CreditForm> AddAsync(CreditForm creditForm);
        Task<CreditForm> UpdateAsync(CreditForm creditForm);
        Task<IEnumerable<CreditForm>> GetByPymeIdAsync(Guid pymeId);
    }
}
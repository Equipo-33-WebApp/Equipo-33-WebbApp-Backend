using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.CreditApplication
{
    public interface ICreditFormRepository
    {
        Task<CreditForm?> GetByIdAsync(Guid id);
    }
}
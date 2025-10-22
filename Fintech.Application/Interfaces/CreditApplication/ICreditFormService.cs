using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.CreditApplication
{
    public interface ICreditFormService
    {
        Task<CreditForm?> GetByIdAsync(Guid id);
    }
}
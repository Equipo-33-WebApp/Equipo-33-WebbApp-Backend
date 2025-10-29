using Fintech.Domain.Entities;

namespace Fintech.Domain.Interfaces;

public interface ICreditFormRepository
{
    Task<IEnumerable<CreditForm>> GetByPymeIdAsync(Guid pymeId);
}
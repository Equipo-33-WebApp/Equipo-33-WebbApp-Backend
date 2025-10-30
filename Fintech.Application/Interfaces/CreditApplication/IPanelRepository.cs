using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface IPanelRepository
{
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(CreditFilter filter);
    Task<CreditForm?> UpdateStatusAsync(Guid creditFormId, string newStatus);
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationByPymeAsync(Guid pymeId, CreditFilter filter);
}

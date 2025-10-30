using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface IPanelRepository
{
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(int page, int pageSize, string status);
    Task<CreditForm?> UpdateStatusAsync(Guid creditFormId, string newStatus);
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationByPymeAsync(Guid pymeId, int page, int pageSize, string status);
}

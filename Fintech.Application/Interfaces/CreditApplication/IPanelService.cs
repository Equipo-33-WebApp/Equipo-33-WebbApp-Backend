using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface IPanelService
{
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(int page, int pageSize, string status);
}
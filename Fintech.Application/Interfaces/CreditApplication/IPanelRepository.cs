using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface IPanelRepository
{
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(int page, int pageSize, string status);
}

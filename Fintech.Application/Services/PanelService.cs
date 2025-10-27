using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Services;

public class PanelService(IPanelRepository _panelRepository) : IPanelService
{
    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(int page, int pageSize, string status)
    {
        return await _panelRepository.GetAllCreditApplicationAsync(page, pageSize, status);
    }
}

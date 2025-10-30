using Fintech.Application.DTOs;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Services;

public class PanelService(IPanelRepository _panelRepository, ICreditFormRepository _creditFormRepository) : IPanelService
{
    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(CreditFilter filter)
    {
        return await _panelRepository.GetAllCreditApplicationAsync(filter);
    }

    public async Task<CreditForm?> UpdateStatusAsync(UpdateStatusCreditFormDto dto, Guid creditFormId)
    {
        var existingCreditForm = await _creditFormRepository.GetByIdAsync(creditFormId);
        if (existingCreditForm == null)
        {
            return null;
        }
        return await _panelRepository.UpdateStatusAsync(creditFormId, dto.Status);
    }

    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationByPymeAsync(Guid pymeId, CreditFilter filter)
    {
        return await _panelRepository.GetAllCreditApplicationByPymeAsync(pymeId, filter);
    }
}

using Fintech.Application.DTOs;
using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface IPanelService
{
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(CreditFilter filter);
    Task<CreditForm?> UpdateStatusAsync(UpdateStatusCreditFormDto dto, Guid creditFormId);
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationByPymeAsync(Guid pymeId, CreditFilter filter);
}
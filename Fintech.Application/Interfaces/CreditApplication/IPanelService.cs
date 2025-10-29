using Fintech.Application.DTOs;
using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface IPanelService
{
    Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(int page, int pageSize, string status);
    Task<CreditForm?> UpdateStatusAsync(UpdateStatusCreditFormDto dto, Guid creditFormId);
}
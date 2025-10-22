using Fintech.Application.DTOs.Aml;

namespace Fintech.Application.Interfaces.Aml;

public interface IAmlService
{
    Task<AmlResultDto> CheckAsync(AmlRequestDto req, Guid authId);
    Task<IEnumerable<AmlResultDto>> GetChecksByAuthIdAsync(Guid authId);
    Task<AmlResultDto?> GetByIdAsync(Guid id);
}
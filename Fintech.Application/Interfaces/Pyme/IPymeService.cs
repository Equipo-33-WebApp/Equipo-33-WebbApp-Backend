using Fintech.Application.DTOs;
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces
{
    public interface IPymeService
    {
        Task<Pyme?> GetByIdAsync(Guid id);
        Task<Pyme?> GetByAuthIdAsync(Guid authId);
        Task<Pyme> CreateAsync(PymeRequestDto dto, Guid authId);
        Task<Pyme?> UpdateAsync(Guid authId, UpdatePymeDto dto);
        Task DeleteAsync(Guid authId);
    }
}

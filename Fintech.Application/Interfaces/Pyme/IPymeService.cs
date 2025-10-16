using Fintech.Application.DTOs;
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces
{
    public interface IPymeService
    {
        Task<Pyme> CreateAsync(PymeRequestDto dto, Guid userId);
        Task<Pyme?> GetByIdAsync(Guid id);
        Task<Pyme?> GetByAuthIdAsync(Guid authId);
    }
}

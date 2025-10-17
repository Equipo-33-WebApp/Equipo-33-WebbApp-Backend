using Fintech.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Fintech.Application.Interfaces
{
    public interface IPymeRepository
    {
        Task<Pyme?> GetByIdAsync(Guid id);
        Task<Pyme?> GetByAuthIdAsync(Guid authId);
        Task<Pyme> AddAsync(Pyme pyme);
        Task<Pyme> UpdateAsync(Pyme pyme);
        Task DeleteAsync(Guid authId);
    }
}

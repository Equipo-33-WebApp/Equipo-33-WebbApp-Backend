using Fintech.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Fintech.Application.Interfaces
{
    public interface IPymeRepository
    {
        Task<Pyme> AddAsync(Pyme pyme);
        Task<Pyme?> GetByIdAsync(Guid id);
    }
}

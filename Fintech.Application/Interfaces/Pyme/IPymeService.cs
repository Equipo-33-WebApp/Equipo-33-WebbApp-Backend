using Fintech.Application.DTOs;
using Fintech.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Fintech.Application.Interfaces
{
    public interface IPymeService
    {
        Task<Pyme> CreateAsync(PymeRequestDto dto, Guid userId);
        Task<Pyme?> GetByIdAsync(Guid id);
    }
}

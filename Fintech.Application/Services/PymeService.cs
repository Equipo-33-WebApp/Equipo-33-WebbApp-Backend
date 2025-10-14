using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Fintech.Application.Services
{
    public class PymeService : IPymeService
    {
        private readonly IPymeRepository _pymeRepository;

        public PymeService(IPymeRepository pymeRepository)
        {
            _pymeRepository = pymeRepository;
        }

        public async Task<Pyme> CreateAsync(PymeRequestDto dto, Guid userId)
        {
            var pyme = new Pyme
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Sector = dto.Sector,
                Employees = dto.Employees,
                Phone = dto.Phone
            };

            return await _pymeRepository.AddAsync(pyme);
        }

        public async Task<Pyme?> GetByIdAsync(Guid id)
        {
            return await _pymeRepository.GetByIdAsync(id);
        }
    }
}

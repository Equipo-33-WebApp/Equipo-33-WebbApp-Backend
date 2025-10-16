using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;

namespace Fintech.Application.Services
{
    public class PymeService : IPymeService
    {
        private readonly IPymeRepository _pymeRepository;

        public PymeService(IPymeRepository pymeRepository)
        {
            _pymeRepository = pymeRepository;
        }

        public async Task<Pyme> CreateAsync(PymeRequestDto dto, Guid authId)
        {
            var pyme = new Pyme
            {
                Id = Guid.NewGuid(),
                AuthId = authId,
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

        public async Task<Pyme?> GetByAuthIdAsync(Guid authId)
        {
            return await _pymeRepository.GetByAuthIdAsync(authId);
        }
    }
}

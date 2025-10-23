using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.DTOs.KycValidation;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;

namespace Fintech.Application.Services
{
    public class PymeService : IPymeService
    {
        private readonly IPymeRepository _pymeRepository;
        private readonly IMapper _mapper;


        public PymeService(IPymeRepository pymeRepository, IMapper mapper)
        {
            _pymeRepository = pymeRepository;
            _mapper = mapper;
        }

        public async Task<Pyme?> GetByIdAsync(Guid id)
        {
            return await _pymeRepository.GetByIdAsync(id);
        }

        public async Task<Pyme?> GetByAuthIdAsync(Guid authId)
        {
            return await _pymeRepository.GetByAuthIdAsync(authId);
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

        public async Task<Pyme?> UpdateAsync(Guid authId, UpdatePymeDto dto)
        {
            var pyme = await _pymeRepository.GetByAuthIdAsync(authId);
            if (pyme == null)
            {
                return null;
            }

            _mapper.Map(dto, pyme);
            var updatedPyme = await _pymeRepository.UpdateAsync(pyme);
            return _mapper.Map<Pyme>(updatedPyme);
        }

        public async Task DeleteAsync(Guid authId)
        {
            await _pymeRepository.DeleteAsync(authId);
        }

        public async Task<bool> VerifyAsync(UpdateKycPymeDto updateKycPymeDto)
        {
            var kycPyme = _mapper.Map<Kyc>(updateKycPymeDto);
            return await _pymeRepository.VerifyAsync(kycPyme);
        }

        public async Task<Kyc?> GetByKycAsync(UpdateKycPymeDto updateKycPymeDto)
        {
            var kycPyme = _mapper.Map<Kyc>(updateKycPymeDto);
            return await _pymeRepository.GetByKycAsync(kycPyme);
        }

        public async Task<Kyc?> GetByNationalIdNumberAsync(string nationalIdNumber)
        {
            return await _pymeRepository.GetByNationalIdNumberAsync(nationalIdNumber);
        }
    }
}

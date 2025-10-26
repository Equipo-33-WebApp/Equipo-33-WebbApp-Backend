using Supabase;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using AutoMapper;

namespace Fintech.Infrastructure.Repositories
{
    public class PymeRepository : IPymeRepository
    {
        private readonly Client _client;
        private readonly IMapper _mapper;

        public PymeRepository(Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<Pyme?> GetByIdAsync(Guid id)
        {
            var result = await _client.From<PymeModel>().Where(x => x.Id == id).Get();
            var model = result.Models.FirstOrDefault();

            return model != null ? _mapper.Map<Pyme>(model) : null;
        }

        public async Task<Pyme?> GetByAuthIdAsync(Guid authId)
        {
            var result = await _client.From<PymeModel>().Where(x => x.AuthId == authId).Get();
            var model = result.Models.FirstOrDefault();

            return model != null ? _mapper.Map<Pyme>(model) : null;
        }

        public async Task<Pyme> AddAsync(Pyme pyme)
        {
            var model = new PymeModel
            {
                Id = pyme.Id,
                AuthId = pyme.AuthId,
                CompanyName = pyme.CompanyName,
                Address = pyme.Address,
                Sector = pyme.Sector,
                Employees = pyme.Employees,
                Phone = pyme.Phone,
                CreatedAt = DateTime.UtcNow
            };

            await _client.From<PymeModel>().Insert(model);
            return pyme;
        }

        public async Task<Pyme> UpdateAsync(Pyme pyme)
        {
            var model = _mapper.Map<PymeModel>(pyme);
            var result = await _client.From<PymeModel>().Update(model);
            return _mapper.Map<Pyme>(result.Models.First());
        }

        public async Task DeleteAsync(Guid authId)
        {
            await _client.From<PymeModel>().Where(x => x.AuthId == authId).Delete();
        }

        public async Task<bool> VerifyAsync(Kyc kyc)
        {
            var user = _client.Auth.CurrentUser;
            if (user == null || string.IsNullOrEmpty(user.Id))
                throw new InvalidOperationException("No authenticated user found.");

            kyc.AuthId = Guid.Parse(user.Id);
            
            var model = _mapper.Map<PymeKycModel>(kyc);

            var response = await _client.From<PymeKycModel>().Update(model);

            return response.Models.Any();
        }

        public async Task<Kyc?> GetByKycAsync(Kyc kyc)
        {
            var result = await _client
                .From<PymeKycModel>()
                .Where(x => x.NationalIdNumber == kyc.NationalIdNumber)
                .Where(x => x.DocumentFrontHash == kyc.DocumentFrontHash)
                .Where(x => x.FaceSelfieHash == kyc.FaceSelfieHash)
                .Get();
            var model = result.Models.FirstOrDefault();

            return model != null ? _mapper.Map<Kyc>(model) : null;
        }

        public async Task<Kyc?> GetByNationalIdNumberAsync(string nationalIdNumber)
        {
            var result = await _client
                .From<PymeKycModel>()
                .Where(x => x.NationalIdNumber == nationalIdNumber)
                .Get();
            var model = result.Models.FirstOrDefault();

            return model != null ? _mapper.Map<Kyc>(model) : null;
        }
    }
}

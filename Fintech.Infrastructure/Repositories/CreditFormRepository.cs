using AutoMapper;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using Supabase;

namespace Fintech.Infrastructure.Repositories
{
    public class CreditFormRepository : ICreditFormRepository
    {
        private readonly Client _client;
        private readonly IMapper _mapper;
        public CreditFormRepository(Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<CreditForm?> GetByIdAsync(Guid id)
        {
            var result = await _client.From<CreditFormModel>()
                .Select("*,pymes(*),uploaded_documents(*)")
                .Where(x => x.Id == id).Get();
            var model = result.Models.FirstOrDefault();

            return model != null ? _mapper.Map<CreditForm>(model) : null;
        }
        public async Task<CreditForm?> GetByAuthIdAsync(Guid authId)
        {
            var result = await _client.From<CreditFormModel>().Where(x => x.UserId == authId).Get();
            var model = result.Models.FirstOrDefault();
            return model != null ? _mapper.Map<CreditForm>(model) : null;
        }
        public async Task<CreditForm> AddAsync(CreditForm creditForm)
        {
            var model = new CreditFormModelCreate
            {
                Id = creditForm.Id,
                UserId = creditForm.UserId,
                PymeId = creditForm.PymeId,
                Amount = creditForm.Amount ?? 0,
                Purpose = creditForm.Purpose ?? string.Empty,
                Status = creditForm.Status ?? "pendiente",
                CreatedAt = DateTime.UtcNow,
            };

            var creditModel = await _client.From<CreditFormModelCreate>().Insert(model);

            creditForm.Id = creditModel.Models.First().Id;
            return creditForm;
        }
    }
}
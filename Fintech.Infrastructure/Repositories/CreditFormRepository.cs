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
            var result = await _client.From<CreditFormModel>().Where(x => x.Id == id).Get();
            var model = result.Models.FirstOrDefault();

            return model != null ? _mapper.Map<CreditForm>(model) : null;
        }
    }
}
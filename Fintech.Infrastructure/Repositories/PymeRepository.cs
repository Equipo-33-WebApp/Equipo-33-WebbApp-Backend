using Supabase;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace Fintech.Infrastructure.Repositories
{
    public class PymeRepository : IPymeRepository
    {
        private readonly Client _client;

        public PymeRepository(Client client)
        {
            _client = client;
        }

        public async Task<Pyme> AddAsync(Pyme pyme)
        {
            var model = new PymeModel
            {
                Id = pyme.Id,
                UserId = pyme.UserId,
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

        public async Task<Pyme?> GetByIdAsync(Guid id)
        {
            var result = await _client.From<PymeModel>().Where(x => x.Id == id).Single();
            if (result == null) return null;

            return new Pyme
            {
                Id = result.Id,
                UserId = result.UserId,
                CompanyName = result.CompanyName,
                Address = result.Address,
                Sector = result.Sector,
                Employees = result.Employees,
                Phone = result.Phone
            };
        }
    }
}

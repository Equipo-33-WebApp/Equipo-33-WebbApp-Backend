using Fintech.Domain.Entities;

namespace Fintech.Domain.Interfaces;

public interface IAmlRepository
{
    Task<IEnumerable<AmlCheck>> GetAllByAuthIdsync(Guid authId);
    Task<AmlCheck?> GetByIdAsync(Guid id);
    Task<AmlCheck> AddAsync(AmlCheck amlCheck);
}
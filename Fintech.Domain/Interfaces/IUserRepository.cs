using Fintech.Domain.Entities;

namespace Fintech.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByAuthIdAsync(Guid authId);
    Task<User?> GetCurrentUserAsync();
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<Guid?> GetPymeIdByUserIdAsync(Guid userId); //para status pyme
}
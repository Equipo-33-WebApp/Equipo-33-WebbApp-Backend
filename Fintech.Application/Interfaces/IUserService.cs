using Fintech.Application.DTOs;
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(User user);
    Task<UserDto> UpdateAsync(User user);
    Task DeleteAsync(Guid id);

}
using Fintech.Application.DTOs;
using Fintech.Application.Services;
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByAuthIdAsync(Guid authId);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto user);
    Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto user);
    Task DeleteAsync(Guid id);
    Task<bool> IsRoleAsync(Roles role);
}
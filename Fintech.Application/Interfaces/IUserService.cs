using Fintech.Application.DTOs;

namespace Fintech.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto user);
    Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto user);
    Task DeleteAsync(Guid id);

}
using Fintech.Application.DTOs;

namespace Fintech.Application.Interfaces;

public interface IAuthService
{
    Task<UserDto?> RegisterAsync(RegisterUserDto registerUser);
    Task<string?> LoginAsync(LoginUserDto loginUserDto);

}
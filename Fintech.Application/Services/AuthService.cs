using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Supabase;

namespace Fintech.Application.Services;

public class AuthService : IAuthService
{
    private readonly Client _supabase;
    private readonly IUserService _userService;

    public AuthService(Client supabase, IUserService userService)
    {
        _supabase = supabase;
        _userService = userService;
    }

    public async Task<UserDto?> RegisterAsync(RegisterUserDto registerUser)
    {
        var session = await _supabase.Auth.SignUp(registerUser.Email, registerUser.Password);
        if (session?.User == null)
        {
            throw new Exception("No se pudo registrar el usuario");
        }

        if (string.IsNullOrEmpty(session.User.Id))
        {
            throw new Exception("El registro de autenticación no devolvió un ID de usuario.");
        }

        var createUserDto = new CreateUserDto
        {
            AuthId = session.User.Id,
            Email = registerUser.Email,
        };

        var createdUser = await _userService.CreateAsync(createUserDto);
        return createdUser;
    }

    public async Task<string?> LoginAsync(LoginUserDto loginUserDto)
    {
        var session = await _supabase.Auth.SignIn(loginUserDto.Email, loginUserDto.Password);
        if (session?.User == null)
        {
            throw new Exception("No se pudo iniciar sesión");
        }

        if (string.IsNullOrEmpty(session.User.Id))
        {
            throw new Exception("El inicio de sesión no devolvió un ID de usuario.");
        }

        if (string.IsNullOrEmpty(session.AccessToken))
        {
            throw new Exception("El inicio de sesión no devolvió un token.");
        };

        return session.AccessToken;
    }
}
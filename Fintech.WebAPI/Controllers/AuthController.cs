using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registrar un nuevo usuario.
    /// </summary>
    /// <param name="registerUser">Email y contraseña del nuevo usuario.</param>
    /// <returns>Información del usuario registrado.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserDto registerUser)
    {
        var user = await _authService.RegisterAsync(registerUser);
        return Ok(user);
    }

    /// <summary>
    /// Iniciar sesión con email y contraseña.
    /// </summary>
    /// <param name="loginUserDto">Email y contraseña del usuario.</param>
    /// <returns>Mensaje de login exitoso y token de autenticación.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserDto loginUserDto)
    {
        var token = await _authService.LoginAsync(loginUserDto);
        if (token == null)
        {
            return Unauthorized("Credenciales inválidas");
        }

        Response.Cookies.Append("auth_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddHours(3),
        });

        return Ok(new { message = "Login exitoso", token });
    }
}
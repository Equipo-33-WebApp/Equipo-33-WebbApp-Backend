using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Obtener un usuario por su ID.
    /// </summary>
    /// <param name="id">ID del usuario a obtener.</param>
    /// <returns>La información del usuario.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto?>> GetByIdAsync(Guid id)
    {
        var response = await _userService.GetByIdAsync(id);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }

    /// <summary>
    /// Obtener todos los usuarios.
    /// </summary>
    /// <returns>La lista de usuarios.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
    {
        var response = await _userService.GetAllAsync();
        return Ok(response);
    }

    /// <summary>
    /// Crear un nuevo usuario.
    /// </summary>
    /// <param name="user">El usuario a crear.</param>
    /// <returns>La información del usuario creado.</returns>
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> CreateAsync(CreateUserDto user)
    {
        var response = await _userService.CreateAsync(user);
        return Ok(response);
    }

    /// <summary>
    /// Actualizar un usuario por su ID.
    /// </summary>
    /// <param name="id">ID del usuario a actualizar.</param>
    /// <param name="userDto">Datos del usuario a actualizar.</param>
    /// <returns>La información del usuario actualizado.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateAsync(Guid id, UpdateUserDto userDto)
    {
        var updatedUser = await _userService.UpdateAsync(id, userDto);
        if (updatedUser == null)
        {
            return NotFound();
        }
        return Ok(updatedUser);
    }

    /// <summary>
    /// Eliminar un usuario por su ID.
    /// </summary>
    /// <param name="id">ID del usuario a eliminar.</param>
    /// <returns>Respuesta sin contenido.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
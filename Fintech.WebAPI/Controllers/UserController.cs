using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetByIdAsync(Guid id)
    {
        var response = await _userService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
    {
        var response = await _userService.GetAllAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateAsync(User user)
    {
        var response = await _userService.CreateAsync(user);
        return Ok(response);
    }

    // TODO: Añadir lógica para actualizar un usuario
    [HttpPut("{id}")]
    public Task<ActionResult<User>> UpdateAsync(Guid id, User user)
    {
        throw new NotImplementedException();
    }

    // TODO: Añadir lógica para eliminar un usuario
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
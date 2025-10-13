using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

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

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
    {
        var response = await _userService.GetAllAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUserDto user)
    {
        var response = await _userService.CreateAsync(user);
        return Ok(response);
    }

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
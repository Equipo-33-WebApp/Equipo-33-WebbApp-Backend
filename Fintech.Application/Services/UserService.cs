using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Domain.Entities;
using Fintech.Domain.Interfaces;

namespace Fintech.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }
        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto createUser)
    {
        var user = _mapper.Map<User>(createUser);
        var createdUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }

    // TODO: Añadir lógica para actualizar un usuario
    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        _mapper.Map(updateUserDto, user);
        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(updatedUser);
    }

    // TODO: Añadir lógica para eliminar un usuario
    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

}
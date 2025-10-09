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

    public async Task<UserDto> CreateAsync(User user)
    {
        var createdUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }

    // TODO: Añadir lógica para actualizar un usuario
    public Task<UserDto> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    // TODO: Añadir lógica para eliminar un usuario
    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
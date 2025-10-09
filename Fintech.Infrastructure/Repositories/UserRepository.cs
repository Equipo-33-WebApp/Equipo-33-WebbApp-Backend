using AutoMapper;
using Fintech.Domain.Entities;
using Fintech.Domain.Interfaces;
using Fintech.Infrastructure.Persistence.Models;
using Supabase;

namespace Fintech.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Client _supabase;
    private readonly IMapper _mapper;

    public UserRepository(Client supabase, IMapper mapper)
    {
        _supabase = supabase;
        _mapper = mapper;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var result = await _supabase.From<UserModel>()
                .Where(u => u.Id == id)
                .Get();

        var model = result.Models.FirstOrDefault();
        return model != null ? _mapper.Map<User>(model) : null;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var result = await _supabase.From<UserModel>().Get();
        var models = result.Models;
        return _mapper.Map<IEnumerable<User>>(models);
    }

    public async Task<User> CreateAsync(User user)
    {
        var model = _mapper.Map<UserModel>(user);
        var inserted = await _supabase.From<UserModel>().Insert(model);
        return _mapper.Map<User>(inserted.Models.First());
    }

    public Task<User> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
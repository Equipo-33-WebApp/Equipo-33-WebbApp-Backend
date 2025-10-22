using AutoMapper;
using Fintech.Domain.Entities;
using Fintech.Domain.Interfaces;
using Fintech.Infrastructure.Persistence.Models;
using Supabase;

namespace Fintech.Infrastructure.Repositories;

public class AmlRepository : IAmlRepository
{
    private readonly Client _supabase;
    private readonly IMapper _mapper;
    public AmlRepository(Client supabase, IMapper mapper)
    {
        _supabase = supabase;
        _mapper = mapper;
    }

    public async Task<AmlCheck> AddAsync(AmlCheck amlCheck)
    {
        var model = _mapper.Map<AmlCheckModel>(amlCheck);
        var inserted = await _supabase.From<AmlCheckModel>().Insert(model);
        return _mapper.Map<AmlCheck>(inserted.Models.First());
    }

    public async Task<IEnumerable<AmlCheck>> GetAllByAuthIdsync(Guid authId)
    {
        var result = await _supabase.From<AmlCheckModel>()
                .Where(a => a.AuthId == authId)
                .Get();

        var models = result.Models;
        return _mapper.Map<IEnumerable<AmlCheck>>(models);
    }

    public async Task<AmlCheck?> GetByIdAsync(Guid id)
    {
        var result = await _supabase.From<AmlCheckModel>()
                .Where(a => a.Id == id)
                .Get();

        var model = result.Models.FirstOrDefault();
        return model != null ? _mapper.Map<AmlCheck>(model) : null;
    }
}
using AutoMapper;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using Supabase;

namespace Fintech.Infrastructure.Repositories;

public class SignatureRepository(Client _supabase, IMapper _mapper) : ISignatureRepository
{
    public async Task<AuditAcceptance> CreateAsync(AuditAcceptance auditAcceptance)
    {
        var user = _supabase.Auth.CurrentUser;
        if (user == null || string.IsNullOrEmpty(user.Id))
            throw new InvalidOperationException("No authenticated user found.");

        var authId = Guid.Parse(user.Id);

        var model = _mapper.Map<AuditAcceptanceModel>(auditAcceptance);
        model.UserId = authId;
        var inserted = await _supabase.From<AuditAcceptanceModel>().Insert(model);
        return _mapper.Map<AuditAcceptance>(inserted.Models.FirstOrDefault());
    }

    public async Task<AuditAcceptance?> GetAsync(AuditAcceptance auditAcceptance)
    {
        var result = await _supabase.From<AuditAcceptanceModel>()
                .Where(u => u.CreditId == auditAcceptance.CreditId)
                .Where(u => u.UserId == auditAcceptance.UserId)
                .Where(u => u.DocumentHash == auditAcceptance.DocumentHash)
                .Get();

        return result != null && result.Models.Any() ? _mapper.Map<AuditAcceptance>(result.Models.First()) : null;
    }
}

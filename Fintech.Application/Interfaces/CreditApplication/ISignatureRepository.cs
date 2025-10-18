using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface ISignatureRepository
{
    Task<AuditAcceptance> CreateAsync(AuditAcceptance auditAcceptance);
    Task<AuditAcceptance?> GetAsync(AuditAcceptance auditAcceptance);
}

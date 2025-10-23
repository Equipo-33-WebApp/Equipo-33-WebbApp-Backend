namespace Fintech.Domain.Entities;

public class Kyc : Entity
{
    public Guid AuthId { get; set; }
    public bool HasKycValidated { get; set; }
    public string NationalIdNumber { get; set; } = string.Empty;
    public string DocumentFrontHash { get; set; } = string.Empty;
    public string FaceSelfieHash { get; set; } = string.Empty;
}

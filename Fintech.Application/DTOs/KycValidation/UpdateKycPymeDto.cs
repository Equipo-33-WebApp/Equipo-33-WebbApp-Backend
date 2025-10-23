namespace Fintech.Application.DTOs.KycValidation;

public class UpdateKycPymeDto
{
    public bool HasKycValidated { get; set; }
    public string NationalIdNumber { get; set; } = string.Empty;
    public string DocumentFrontHash { get; set; } = string.Empty;
    public string FaceSelfieHash { get; set; } = string.Empty;
}

namespace Fintech.Application.DTOs.KycValidation;

public class VerifiedDocumentFaceDto
{
    public VerifiedFacialRecognitionDto? VerificationResult { get; set; }
    public string? Error { get; set; }
}

public class KycVerificationResultDto
{
    public bool Verified { get; set; }
    public string Percentage { get; set; }
    public string Observation { get; set; }
}

public class KycVerificationRequestDto
{
    public Stream IdDocumentFront { get; set; }
    public string IdDocumentFrontName { get; set; }
    public string IdDocumentFrontContentType { get; set; }
    public Stream FaceSelfie { get; set; }
    public string FaceSelfieName { get; set; }
    public string FaceSelfieContentType { get; set; }
    public string NationalIdNumber { get; set; }
}

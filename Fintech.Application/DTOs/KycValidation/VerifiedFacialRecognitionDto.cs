namespace Fintech.Application.DTOs.KycValidation;

public class VerifiedFacialRecognitionDto
{
    public bool Verified { get; set; }
    public float Distance { get; set; }
    public float Threshold { get; set; }
    public string Model { get; set; }
    public string DetectorBackend { get; set; }
    public string SimilarityMetric { get; set; }
}

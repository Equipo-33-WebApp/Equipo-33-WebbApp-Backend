namespace Fintech.Application.DTOs.KycValidation;

public class FoundFacialRecognitionDto
{
    public FoundFacialRecognitionItemDto[] Result { get; set; }
}

public class FoundFacialRecognitionItemDto
{
    public string Identity { get; set; }
    public float EuclideanDistance { get; set; }
    public string Model { get; set; }
    public string DetectorBackend { get; set; }
    public string SimilarityMetric { get; set; }
}

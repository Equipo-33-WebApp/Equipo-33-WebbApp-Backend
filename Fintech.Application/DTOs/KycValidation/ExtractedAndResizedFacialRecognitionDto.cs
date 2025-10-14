namespace Fintech.Application.DTOs.KycValidation;

public class ExtractedFacialRecognitionDto
{
    public byte[]? FaceImage { get; set; }
    public string? Error { get; set; }
}

public class ResizedImageDto
{
    public byte[]? ResizedImage { get; set; }
    public string? ContentType { get; set; }
    public string? FileName { get; set; }
    public string? Error { get; set; }
}

public class ResizeImageRequestDto
{
    public byte[]? Img { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
}

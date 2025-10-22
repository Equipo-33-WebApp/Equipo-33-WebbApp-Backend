namespace Fintech.Application.DTOs.Aml;

public class AmlRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
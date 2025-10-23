namespace Fintech.Application.DTOs.Aml;

public class GeminiAmlRequestDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public int Employees { get; set; } = 0;
    public string Phone { get; set; } = string.Empty;
}
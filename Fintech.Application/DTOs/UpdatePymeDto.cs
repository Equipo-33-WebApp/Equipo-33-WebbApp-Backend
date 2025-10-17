namespace Fintech.Application.DTOs;

public class UpdatePymeDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public int Employees { get; set; }
    public string Phone { get; set; } = string.Empty;
}
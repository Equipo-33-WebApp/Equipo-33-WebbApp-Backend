namespace Fintech.Application.DTOs;

public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Role { get; set; } = "PYME";
}
namespace Fintech.Application.DTOs;

public class StatusPymeCountDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
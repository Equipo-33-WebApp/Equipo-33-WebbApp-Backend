namespace Fintech.Application.DTOs;

public class StatusPymeSummaryDto
{
    public Guid PymeId { get; set; }
    public Guid UserId { get; set; }
    public int Total { get; set; }
    public List<StatusPymeCountDto> StatusCounts { get; set; } = new();
    public Dictionary<string, int> CategoryTotals { get; set; } = new();
}
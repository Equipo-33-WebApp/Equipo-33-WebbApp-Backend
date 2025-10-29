using Fintech.Application.DTOs;

namespace Fintech.Application.Interfaces;

public interface IStatusPymeService
{
    Task<StatusPymeSummaryDto> GetStatusSummaryAsync();
}
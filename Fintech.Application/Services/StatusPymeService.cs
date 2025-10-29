using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Domain.Interfaces;
using CreditFormRepo = Fintech.Application.Interfaces.CreditApplication.ICreditFormRepository; 

namespace Fintech.Application.Services;

public class StatusPymeService : IStatusPymeService
{
    private readonly CreditFormRepo _creditFormRepository; 
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public StatusPymeService(
        CreditFormRepo creditFormRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _creditFormRepository = creditFormRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<StatusPymeSummaryDto> GetStatusSummaryAsync()
    {
        var userId = _currentUserService.GetUserId();
        var pymeId = await _userRepository.GetPymeIdByUserIdAsync(userId);

        if (!pymeId.HasValue)
        {
            throw new InvalidOperationException("El usuario no tiene una PYME asignada");
        }

        var creditForms = await _creditFormRepository.GetByPymeIdAsync(pymeId.Value);
        var formsList = creditForms.ToList();

        var statusGroups = formsList
            .Where(f => !string.IsNullOrEmpty(f.Status))
            .GroupBy(f => f.Status!.ToLower())
            .Select(g => new StatusPymeCountDto
            {
                Status = g.Key,
                Count = g.Count(),
                DisplayName = GetStatusDisplayName(g.Key),
                Category = GetStatusCategory(g.Key)
            })
            .OrderBy(s => GetStatusOrder(s.Status))
            .ToList();

        var allStatuses = new[] { "draft", "pendiente", "revision", "aprobado", "rechazado" };
        foreach (var status in allStatuses)
        {
            if (!statusGroups.Any(s => s.Status == status))
            {
                statusGroups.Add(new StatusPymeCountDto
                {
                    Status = status,
                    Count = 0,
                    DisplayName = GetStatusDisplayName(status),
                    Category = GetStatusCategory(status)
                });
            }
        }

        statusGroups = statusGroups.OrderBy(s => GetStatusOrder(s.Status)).ToList();

        var categoryTotals = statusGroups
            .GroupBy(s => s.Category)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Count));

        return new StatusPymeSummaryDto
        {
            PymeId = pymeId.Value,
            UserId = userId,
            Total = formsList.Count,
            StatusCounts = statusGroups,
            CategoryTotals = categoryTotals
        };
    }

    private string GetStatusDisplayName(string status)
    {
        return status.ToLower() switch
        {
            "draft" => "Borrador",
            "pendiente" => "Pendiente",
            "revision" => "En Revisión",
            "aprobado" => "Aprobado",
            "rechazado" => "Rechazado",
            _ => status
        };
    }

    private string GetStatusCategory(string status)
    {
        return status.ToLower() switch
        {
            "draft" => "Borradores",
            "pendiente" or "revision" => "En Proceso",
            "aprobado" => "Aprobados",
            "rechazado" => "Rechazados",
            _ => "Otros"
        };
    }

    private int GetStatusOrder(string status)
    {
        return status.ToLower() switch
        {
            "draft" => 1,
            "pendiente" => 2,
            "revision" => 3,
            "aprobado" => 4,
            "rechazado" => 5,
            _ => 99
        };
    }
}
namespace Fintech.Application.Interfaces.Aml;

public interface IGeminiAmlService
{
    Task<(string RiskLevel, string ResultSummary)> AnalyzeAsync(string fullName, string documentNumber, string country);
}
using Fintech.Application.Interfaces.Aml;
using GenerativeAI;

namespace Fintech.Infrastructure.Service;

public class GeminiAmlService : IGeminiAmlService
{
    private readonly string _apiKey;

    public GeminiAmlService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<(string RiskLevel, string ResultSummary)> AnalyzeAsync(string fullName, string documentNumber, string country)
    {
        var googleAI = new GoogleAi(_apiKey);
        var model = googleAI.CreateGenerativeModel("models/gemini-2.5-flash");

        string prompt = $"Analiza el riesgo AML de la persona con nombre completo '{fullName}', número de documento '{documentNumber}' y país '{country}'. " +
                        "Clasifica el riesgo como Bajo, Medio o Alto y proporciona una breve justificación para la clasificación.";

        var response = await model.GenerateContentAsync(prompt);
        if (response == null || response?.Candidates?.Length == 0)
        {
            throw new Exception("No se recibió respuesta del modelo generativo.");
        }

        var text = response?.Candidates?.First().Content?.Parts.First().Text;
        if (string.IsNullOrEmpty(text))
        {
            throw new Exception("La respuesta del modelo generativo está vacía.");
        }

        string risk = "Medio";
        if (text.Contains("Bajo", StringComparison.OrdinalIgnoreCase))
        {
            risk = "Bajo";
        }
        else if (text.Contains("Alto", StringComparison.OrdinalIgnoreCase))
        {
            risk = "Alto";
        }

        return (risk, text);
    }
}
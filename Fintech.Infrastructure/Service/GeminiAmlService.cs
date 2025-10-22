using System.Text.Json;
using System.Text.RegularExpressions;
using Fintech.Application.DTOs.Aml;
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

    public async Task<GeminiAmlResponseDto> AnalyzeAsync(GeminiAmlRequestDto req)
    {
        var googleAI = new GoogleAi(_apiKey);
        var model = googleAI.CreateGenerativeModel("models/gemini-2.5-flash");

        string prompt = $@"Eres un analista AML (Anti-Money Laundering). 
        Evalúa el riesgo de la siguiente PYME según su información: 
        Empresa: {req.CompanyName} 
        Dirección: {req.Address} 
        Sector: {req.Sector} 
        Empleados: {req.Employees} 
        Teléfono: {req.Phone}. 
        
        Responde únicamente en JSON con la siguiente estructura: 
        {{
        ""riskLevel"": ""Bajo | Medio | Alto"",
        ""flags"": [""string"", ""string""],
        ""summary"": ""string"",
        ""requiresManualReview"": true | false
        }}
        ";

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


        var cleanJson = Regex.Replace(text, @"^```json\s*|\s*```$", "").Trim();

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var amlResponse = JsonSerializer.Deserialize<GeminiAmlResponseDto>(cleanJson, options);
            return amlResponse ?? throw new Exception("La deserialización del JSON de Gemini resultó en un objeto nulo.");
        }
        catch (JsonException ex)
        {
            throw new Exception($"Error al deserializar la respuesta JSON de Gemini. Respuesta recibida: {cleanJson}", ex);
        }
    }
}
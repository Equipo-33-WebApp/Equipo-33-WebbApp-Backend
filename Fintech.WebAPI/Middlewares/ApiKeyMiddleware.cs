using DotNetEnv;

namespace Fintech.WebAPI.Middlewares;

/// <summary>
/// Middleware para validar la API key en las solicitudes.
/// </summary>
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    { 
        _next = next;
        // Leemos la API Key directamente desde la configuración (appsettings.json, variables de entorno, etc.)
        _apiKey = configuration["ApiKey"] ?? throw new Exception("La configuración 'ApiKey' no fue encontrada.");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key es requerida.");
            return;
        }

        if (!_apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("API Key inválida.");
            return;
        }

        await _next(context);
    }
}
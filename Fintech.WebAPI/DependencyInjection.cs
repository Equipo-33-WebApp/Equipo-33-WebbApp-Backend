using Fintech.Application.Interfaces;
using Fintech.Application.Services;
using Fintech.Infrastructure.Interfaces;
using Fintech.Infrastructure.Service;
using Refit;

namespace Fintech.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureKycServices(this IServiceCollection services)
    {
        var deepFaceApiBaseUrl = Environment.GetEnvironmentVariable("DEEPFACE_API_URL") ?? "http://localhost:5000";

        services.AddRefitClient<IDeepFaceApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(deepFaceApiBaseUrl));

        var ocrSpaceApiBaseUrl = Environment.GetEnvironmentVariable("OCRSPACE_API_URL") ?? "https://api.ocr.space";

        services.AddRefitClient<IOcrSpaceApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(ocrSpaceApiBaseUrl));

        services.AddScoped<IFacialRecognitionService, DeepFaceService>();
        services.AddScoped<ITextRecognitionService, OcrSpaceService>();
        services.AddScoped<IKycVerificationService, KycVerificationService>();

        return services;
    }
}

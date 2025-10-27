using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Application.Services;
using Fintech.Infrastructure.Interfaces;
using Fintech.Infrastructure.MappingProfiles;
using Fintech.Infrastructure.Repositories;
using Fintech.Infrastructure.Service;
using Refit;

namespace Fintech.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = false;
        });
        services.ConfigureKycServices();
        services.ConfigureAuditServices();
        services.ConfigurePanelServices();
        return services;
    }

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

        services.AddAutoMapper(cfg => { }, typeof(KycProfile));

        return services;
    }

    public static IServiceCollection ConfigureAuditServices(this IServiceCollection services)
    {
        services.AddScoped<ISignatureRepository, SignatureRepository>();
        services.AddScoped<ISignatureService, SignatureService>();

        services.AddAutoMapper(cfg => { }, typeof(AuditAcceptanceProfile));

        return services;
    }

    public static IServiceCollection ConfigurePanelServices(this IServiceCollection services)
    {
        services.AddScoped<IPanelRepository, PanelRepository>();
        services.AddScoped<IPanelService, PanelService>();

        services.AddAutoMapper(cfg => { }, typeof(PanelProfile));

        return services;
    }
}

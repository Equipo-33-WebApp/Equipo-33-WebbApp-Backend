using Refit;
using Fintech.Application.DTOs.KycValidation;

namespace Fintech.Infrastructure.Interfaces;

public interface IDeepFaceApi
{
    [Multipart]
    [Post("/verify")]
    Task<VerifiedFacialRecognitionDto> Verify(
        [AliasAs("img1")] StreamPart img1,
        [AliasAs("img2")] StreamPart img2,
        [AliasAs("model_name")] string modelName = "ArcFace",
        [AliasAs("detector_backend")] string detectorBackend = "retinaface"
    );

    [Multipart]
    [Post("/analyze")]
    Task<AnalyzedFacialRecognitionDto[]> Analyze(
        [AliasAs("img")] StreamPart img,
        [AliasAs("actions")] string actions = "age,gender,race,emotion",
        [AliasAs("model_name")] string modelName = "ArcFace",
        [AliasAs("detector_backend")] string detectorBackend = "retinaface"
    );

    [Multipart]
    [Post("/detect")]
    Task<DetectedFacialRecognitionDto> Detect(
        [AliasAs("img")] StreamPart img,
        [AliasAs("detector_backend")] string detectorBackend = "retinaface"
    );

    [Multipart]
    [Post("/find")]
    Task<FoundFacialRecognitionDto[]> Find(
        [AliasAs("img")] StreamPart img,
        [AliasAs("db_path")] string dbPath = "database",
        [AliasAs("model_name")] string modelName = "ArcFace",
        [AliasAs("detector_backend")] string detectorBackend = "retinaface"
    );

    [Multipart]
    [Post("/extract_face")]
    Task<HttpResponseMessage> ExtractFace(
        [AliasAs("id_document")] StreamPart idDocument,
        [AliasAs("detector_backend")] string detectorBackend = "retinaface",
        [AliasAs("output_format")] string outputFormat = "bytes"
    );

    [Multipart]
    [Post("/resize_image")]
    Task<HttpResponseMessage> ResizeImage(
        [AliasAs("img")] StreamPart img
    );
}

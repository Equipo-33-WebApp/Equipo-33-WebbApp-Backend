using Fintech.Application.DTOs.KycValidation;

namespace Fintech.Application.Interfaces;

public interface IFacialRecognitionService
{
    Task<VerifiedFacialRecognitionDto> Verify(
        Stream img1Stream,
        string img1FileName,
        string img1ContentType,
        Stream img2Stream,
        string img2FileName,
        string img2ContentType,
        string modelName,
        string detectorBackend
    );

    Task<ExtractedFacialRecognitionDto> ExtractFace(
        Stream idDocumentStream,
        string idDocumentFileName,
        string idDocumentContentType,
        string detectorBackend
    );

    Task<VerifiedDocumentFaceDto> VerifyDocumentAndSelfie(
        Stream idDocumentStream,
        string idDocumentFileName,
        string idDocumentContentType,
        Stream selfieStream,
        string selfieFileName,
        string selfieContentType,
        string modelName,
        string detectorBackend
    );

    Task<ResizedImageDto> ResizeImage(
        Stream imgStream,
        string imgFileName,
        string imgContentType
    );
}

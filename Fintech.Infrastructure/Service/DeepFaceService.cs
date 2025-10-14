using Fintech.Application.DTOs.KycValidation;
using Fintech.Application.Interfaces;
using Fintech.Infrastructure.Interfaces;
using Refit;

namespace Fintech.Infrastructure.Service;

public class DeepFaceService : IFacialRecognitionService
{
    private readonly IDeepFaceApi _deepFaceApi;

    public DeepFaceService(IDeepFaceApi deepFaceApi)
    {
        _deepFaceApi = deepFaceApi;
    }

    public async Task<VerifiedFacialRecognitionDto> Verify(
        Stream img1Stream,
        string img1FileName,
        string img1ContentType,
        Stream img2Stream,
        string img2FileName,
        string img2ContentType,
        string modelName,
        string detectorBackend)
    {
        var stream1 = new StreamPart(img1Stream, img1FileName, img1ContentType);
        var stream2 = new StreamPart(img2Stream, img2FileName, img2ContentType);

        var result = await _deepFaceApi.Verify(stream1, stream2, modelName, detectorBackend);
        return result;
    }

    public async Task<ExtractedFacialRecognitionDto> ExtractFace(
        Stream idDocumentStream,
        string idDocumentFileName,
        string idDocumentContentType,
        string detectorBackend)
    {
        var streamPart = new StreamPart(idDocumentStream, idDocumentFileName, idDocumentContentType);
        var response = await _deepFaceApi.ExtractFace(streamPart, detectorBackend);

        if (response.IsSuccessStatusCode)
        {
             var imageBytes = await response.Content.ReadAsByteArrayAsync();
            return new ExtractedFacialRecognitionDto { FaceImage = imageBytes };
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return new ExtractedFacialRecognitionDto { Error = $"DeepFace API Error: {response.StatusCode} - {errorContent}" };
        }
    }

    public async Task<VerifiedDocumentFaceDto> VerifyDocumentAndSelfie(
        Stream idDocumentStream,
        string idDocumentFileName,
        string idDocumentContentType,
        Stream selfieStream,
        string selfieFileName,
        string selfieContentType,
        string modelName,
        string detectorBackend)
    {
        var extractFaceResult = await ExtractFace(
            idDocumentStream,
            idDocumentFileName,
            idDocumentContentType,
            detectorBackend
        );

        if (!string.IsNullOrEmpty(extractFaceResult.Error) || extractFaceResult.FaceImage == null)
        {
            return new VerifiedDocumentFaceDto { Error = extractFaceResult.Error ?? "Failed to extract face from document." };
        }

        using var extractedFaceStream = new MemoryStream(extractFaceResult.FaceImage);

        var verifyResult = await Verify(
            extractedFaceStream,
            "extracted_face.png",
            "image/png",
            selfieStream,
            selfieFileName,
            selfieContentType,
            modelName,
            detectorBackend
        );

        return new VerifiedDocumentFaceDto { VerificationResult = verifyResult };
    }

    public async Task<ResizedImageDto> ResizeImage(
        Stream imgStream,
        string imgFileName,
        string imgContentType)
    {
        var streamPart = new StreamPart(imgStream, imgFileName, imgContentType);
        var response = await _deepFaceApi.ResizeImage(streamPart);

        if (response.IsSuccessStatusCode)
        {
            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            return new ResizedImageDto
            {
                ResizedImage = imageBytes,
                ContentType = response.Content.Headers.ContentType?.MediaType,
                FileName = response.Content.Headers.ContentDisposition?.FileNameStar ?? response.Content.Headers.ContentDisposition?.FileName
            };
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return new ResizedImageDto { Error = $"Resize Image API Error: {response.StatusCode} - {errorContent}" };
        }
    }
}

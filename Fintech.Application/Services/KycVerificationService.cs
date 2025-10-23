using Fintech.Application.DTOs.KycValidation;
using Fintech.Application.Interfaces;
using System.Text.RegularExpressions;

namespace Fintech.Application.Services;

public class KycVerificationService(IFacialRecognitionService _deepFaceService, ITextRecognitionService _ocrSpaceService, IPymeService _pymeService) : IKycVerificationService
{
    public async Task<KycVerificationResultDto> VerifyDocumentAndSelfieAndExtractData(
        KycVerificationRequestDto request)
    {
        var verificationResult = await PerformDeepFaceVerificationAsync(
            _deepFaceService,
            request
        );

        if (!string.IsNullOrEmpty(verificationResult.Error))
        {
            return new KycVerificationResultDto
            {
                Verified = false,
                Percentage = "0%",
                Observation = "El selfie y la foto del documento proporcionado no coinciden."
            };
        }

        var ocrText = await ProcessOcrAsync(
            _deepFaceService,
            _ocrSpaceService,
            request.IdDocumentFront,
            request.IdDocumentFrontName,
            request.IdDocumentFrontContentType
        );

        var verified = verificationResult.VerificationResult?.Verified ?? false;
        float percentage = CalculateVerificationPercentage(verificationResult);

        var validationResult = ValidateNationalIdInOcrText(
            verified,
            percentage,
            ocrText,
            request.NationalIdNumber
        );

        if (validationResult.Verified && verified)
        {
            var result = await _pymeService.VerifyAsync();
            if(!result) validationResult.Observation = "Error al actualizar los registros.";
            validationResult.Observation = "La validación fue exitosa: Identificación nacional y selfie verificados.";
        }

        return validationResult;
    }

    private static float CalculateVerificationPercentage(VerifiedDocumentFaceDto verificationResult)
    {
        var verified = verificationResult.VerificationResult?.Verified ?? false;
        float percentage = 0;

        if (verified && verificationResult.VerificationResult != null)
        {
            var distance = verificationResult.VerificationResult.Distance;
            var threshold = verificationResult.VerificationResult.Threshold;
            percentage = Math.Max(0, (1 - (distance / threshold)) * 100);
        }
        return percentage;
    }

    private static async Task<VerifiedDocumentFaceDto> PerformDeepFaceVerificationAsync(
        IFacialRecognitionService deepFaceService,
        KycVerificationRequestDto request)
    {
        using var idDocumentStreamForDeepFace = new MemoryStream();
        await request.IdDocumentFront.CopyToAsync(idDocumentStreamForDeepFace);
        idDocumentStreamForDeepFace.Seek(0, SeekOrigin.Begin);

        using var selfieStreamForDeepFace = new MemoryStream();
        await request.FaceSelfie.CopyToAsync(selfieStreamForDeepFace);
        selfieStreamForDeepFace.Seek(0, SeekOrigin.Begin);

        var verificationResult = await deepFaceService.VerifyDocumentAndSelfie(
            idDocumentStreamForDeepFace,
            request.IdDocumentFrontName,
            request.IdDocumentFrontContentType,
            selfieStreamForDeepFace,
            request.FaceSelfieName,
            request.FaceSelfieContentType,
            modelName: "ArcFace",
            detectorBackend: "retinaface"
        );
        return verificationResult;
    }

    private static async Task<string> ProcessOcrAsync(
        IFacialRecognitionService deepFaceService,
        ITextRecognitionService ocrSpaceService,
        Stream idDocumentStream,
        string idDocumentFileName,
        string idDocumentContentType)
    {
        idDocumentStream.Seek(0, SeekOrigin.Begin);
        using var ocrStreamForResize = new MemoryStream();
        await idDocumentStream.CopyToAsync(ocrStreamForResize);
        ocrStreamForResize.Seek(0, SeekOrigin.Begin);

        var resizeResult = await deepFaceService.ResizeImage(ocrStreamForResize, idDocumentFileName, idDocumentContentType);

        Stream finalOcrStream;
        string finalOcrFileName = idDocumentFileName;
        string finalOcrContentType = idDocumentContentType;

        if (resizeResult.Error != null || resizeResult.ResizedImage == null)
        {
            idDocumentStream.Seek(0, SeekOrigin.Begin);
            finalOcrStream = idDocumentStream;
        }
        else
        {
            var resizedMemoryStream = new MemoryStream(resizeResult.ResizedImage);
            resizedMemoryStream.Seek(0, SeekOrigin.Begin);
            finalOcrStream = resizedMemoryStream;
            finalOcrFileName = resizeResult.FileName;
            finalOcrContentType = resizeResult.ContentType;
        }

        var ocrResponse = await ocrSpaceService.ParseImageAsync(
            finalOcrStream,
            finalOcrFileName,
            finalOcrContentType
        );

        return ExtractAndCleanOcrText(ocrResponse);
    }

    private static string ExtractAndCleanOcrText(RecognizedTextDto ocrResponse)
    {
        var ocrText = "";
        if (ocrResponse.ParsedResults != null && ocrResponse.ParsedResults.Any())
        {
            ocrText = string.Join(" ", ocrResponse.ParsedResults.Select(r => r.ParsedText));
        }

        ocrText = Regex.Replace(ocrText, @"\s+", " ").Trim();
        return ocrText;
    }

    private static KycVerificationResultDto ValidateNationalIdInOcrText(
        bool currentVerifiedStatus,
        float currentPercentage,
        string ocrText,
        string nationalIdNumber)
    {
        string observationMessage = ocrText;

        if (!ocrText.Contains(nationalIdNumber, StringComparison.OrdinalIgnoreCase))
        {
            currentVerifiedStatus = false;
            observationMessage = "El documento ingresado no coincide con el número de identificación nacional proporcionado.";
            currentPercentage = 0;
        }

        return new KycVerificationResultDto
        {
            Verified = currentVerifiedStatus,
            Percentage = $"{currentPercentage:F0}%",
            Observation = observationMessage
        };
    }
}

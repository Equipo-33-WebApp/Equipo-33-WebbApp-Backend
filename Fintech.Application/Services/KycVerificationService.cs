using Fintech.Application.DTOs.KycValidation;
using Fintech.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Fintech.Application.Services;

public class KycVerificationService(IFacialRecognitionService _deepFaceService, ITextRecognitionService _ocrSpaceService, IPymeService _pymeService) : IKycVerificationService
{
    public async Task<KycVerificationResultDto> VerifyDocumentAndSelfieAndExtractData(
        KycVerificationRequestDto request)
    {
        /* TODO: Comentado solo para la demo, descomentar
        var doesExistPymeByNationalIdNumber = await _pymeService.GetByNationalIdNumberAsync(request.NationalIdNumber);
        if(doesExistPymeByNationalIdNumber != null)
        {
            return new KycVerificationResultDto
            {
                Verified = false,
                Percentage = $"-",
                Observation = "El documento ya se encuentra registrado.",
            };
        }
        */
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
            var result = await ProcessKycUpdateAsync(request, _pymeService);
            if(!result) validationResult.Observation = "Error al actualizar los registros.";
            else validationResult.Observation = "La validación fue exitosa: Identificación nacional y selfie verificados.";
        }

        return validationResult;
    }

    private static async Task<bool> ProcessKycUpdateAsync(KycVerificationRequestDto request, IPymeService pymeService)
    {
        request.IdDocumentFront.Seek(0, SeekOrigin.Begin);
        using var streamDoc = new MemoryStream();
        await request.IdDocumentFront.CopyToAsync(streamDoc);
        streamDoc.Seek(0, SeekOrigin.Begin);

        request.FaceSelfie.Seek(0, SeekOrigin.Begin);
        using var streamSelfie = new MemoryStream();
        await request.FaceSelfie.CopyToAsync(streamSelfie);
        streamSelfie.Seek(0, SeekOrigin.Begin);
        var updateKycPymeDto = new UpdateKycPymeDto()
        {
            HasKycValidated = true,
            NationalIdNumber = request.NationalIdNumber,
            DocumentFrontHash = await HashHelper.ComputeSha256Async(streamDoc),
            FaceSelfieHash = await HashHelper.ComputeSha256Async(streamSelfie),
        };
        var result = await pymeService.VerifyAsync(updateKycPymeDto);
        return result;
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

    public async Task<bool> GetKycInfo(KycVerificationRequestDto request)
    {
        var updateKycPymeDto = new UpdateKycPymeDto()
        {
            NationalIdNumber = request.NationalIdNumber,
            DocumentFrontHash = await HashHelper.ComputeSha256Async(request.IdDocumentFront),
            FaceSelfieHash = await HashHelper.ComputeSha256Async(request.FaceSelfie),
        };
        var result = await _pymeService.GetByKycAsync(updateKycPymeDto);
        return result != null;
    }
}

public static class HashHelper
{
    public static string ComputeSha256(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        using var sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static async Task<string> ComputeSha256Async(Stream stream)
    {
        if (stream == null || stream.Length == 0)
            return string.Empty;

        using var sha256 = SHA256.Create();
        byte[] hash = await sha256.ComputeHashAsync(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}

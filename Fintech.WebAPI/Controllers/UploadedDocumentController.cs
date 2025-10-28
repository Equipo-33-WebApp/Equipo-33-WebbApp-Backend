using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Application.Interfaces.UploadedDocuments;
using Fintech.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.ComponentModel.DataAnnotations;

namespace Fintech.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadedDocumentController : ControllerBase
    {
        private readonly Client _supabaseClient;
        private readonly IStorageDocumentService _storageDocumentService;
        private readonly ICreditFormService _creditFormService;
        private readonly IUploadedDocumentService _uploadedDocumentService;
        public UploadedDocumentController(
            Client supabaseClient,
            IStorageDocumentService storageDocumentService,
            ICreditFormService creditFormService,
            IUploadedDocumentService uploadedDocumentService)
        {
            _supabaseClient = supabaseClient;
            _storageDocumentService = storageDocumentService;
            _creditFormService = creditFormService;
            _uploadedDocumentService = uploadedDocumentService;
        }
        /// <summary>
        /// Sube un archivo annualFinancials al servidor.
        /// </summary>
        /// <param name="creditFormId">ID del credit form asociado (se pasa en la URL).</param>
        /// /// <param name="request">
        /// Form-data con el archivo (campo "File")
        /// El tamaño máximo permitido es 2 MB.
        /// </param>
        /// <returns>Archivo subido correctamente o error correspondiente.</returns>
        [HttpPost("upload/annualFinancials/{creditFormId:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAnnualFinancials([FromRoute] Guid creditFormId,[FromForm] DocumentAddRequestDto request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No se envió ningún archivo.");

            if (request.File.Length > 2 * 1024 * 1024)
                return BadRequest("El archivo excede el tamaño máximo permitido (2 MB).");

            //validar si el id del credit form existe
            var creditForm = await _creditFormService.GetByIdAsync(creditFormId);
            if (creditForm == null)
                return NotFound("El credit form especificado no existe.");

            var bucketName = "credit-documents";
            var bucket = _supabaseClient.Storage.From(bucketName);

            var folderPath = $"{creditFormId}/annualFinancials/";
            var fileName = Path.GetFileName(request.File.FileName).Replace(" ", "_");
            var filePath = folderPath + fileName;

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            try
            {
                // 1. Eliminar archivos existentes en la carpeta (para mantener solo 1)
                await _storageDocumentService.DeleteFilesInDirectoryAsync(bucketName, folderPath);
                //2. Eliminar registros existentes en la tabla UploadedDocuments
                await _uploadedDocumentService.DeleteUploadedDocumentRecordsAsync(creditFormId, folderPath);
                // 3. Subir el nuevo archivo
                var url = await _storageDocumentService.UploadFileAsync(bucketName, filePath, fileBytes);
                // 4. Insertar registro en la tabla UploadedDocuments
                await _uploadedDocumentService.AddAsync(creditFormId, url);

                return Ok(new
                {
                    Message = "Archivo subido correctamente.",
                    FilePath = filePath,
                    Url = url
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error al subir el archivo.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Sube un archivo taxReturn al servidor.
        /// </summary>
        /// <param name="creditFormId">ID del credit form asociado (se pasa en la URL).</param>
        /// /// <param name="request">
        /// Form-data con el archivo (campo "File")
        /// El tamaño máximo permitido es 2 MB.
        /// </param>
        /// <returns>Archivo subido correctamente o error correspondiente.</returns>
        [HttpPost("upload/taxReturn/{creditFormId:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTaxReturn([FromRoute] Guid creditFormId, [FromForm] DocumentAddRequestDto request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No se envió ningún archivo.");

            if (request.File.Length > 2 * 1024 * 1024)
                return BadRequest("El archivo excede el tamaño máximo permitido (2 MB).");

            //validar si el id del credit form existe
            var creditForm = await _creditFormService.GetByIdAsync(creditFormId);
            if (creditForm == null)
                return NotFound("El credit form especificado no existe.");

            var bucketName = "credit-documents";
            var bucket = _supabaseClient.Storage.From(bucketName);

            var folderPath = $"{creditFormId}/taxReturn/";
            var fileName = Path.GetFileName(request.File.FileName).Replace(" ", "_");
            var filePath = folderPath + fileName;

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            try
            {
                // 1. Eliminar archivos existentes en la carpeta (para mantener solo 1)
                await _storageDocumentService.DeleteFilesInDirectoryAsync(bucketName, folderPath);
                //2. Eliminar registros existentes en la tabla UploadedDocuments
                await _uploadedDocumentService.DeleteUploadedDocumentRecordsAsync(creditFormId, folderPath);
                // 3. Subir el nuevo archivo
                var url = await _storageDocumentService.UploadFileAsync(bucketName, filePath, fileBytes);
                // 4. Insertar registro en la tabla UploadedDocuments
                await _uploadedDocumentService.AddAsync(creditFormId, url);

                return Ok(new
                {
                    Message = "Archivo subido correctamente.",
                    FilePath = filePath,
                    Url = url
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error al subir el archivo.",
                    Error = ex.Message
                });
            }
        }
    }

    public class DocumentAddRequestDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}

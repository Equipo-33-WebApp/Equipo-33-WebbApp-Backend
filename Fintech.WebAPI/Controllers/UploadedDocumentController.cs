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
        private readonly Client _storage;
        public UploadedDocumentController(Client client)
        {
            _storage = client;
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

            var bucket = _storage.Storage.From("credit-documents");

            var folderPath = $"{creditFormId}/annualFinancials/";
            var fileName = Path.GetFileName(request.File.FileName).Replace(" ", "_");
            var filePath = folderPath + fileName;

            try
            {
                // 1. Eliminar archivos existentes en la carpeta (para mantener solo 1)
                var existingFiles = await bucket.List(folderPath);
                if (existingFiles != null && existingFiles.Count > 0)
                {
                    var deletePaths = existingFiles.Select(f => folderPath + f.Name)
                        .ToList();
                    await bucket.Remove(deletePaths);
                }
                //2. Eliminar registros existentes en la tabla UploadedDocuments
                var model = _storage.From<UploadedDocumentModel>();
                var existingRecord = await model
                    .Where(x => x.CreditFormId == creditFormId && x.FileUrl.Contains($"/{creditFormId}/annualFinancials/"))
                    .Get();
                if (existingRecord.Models.Count > 0)
                {
                    foreach (var record in existingRecord.Models)
                    {
                        await model.Delete(record);
                    }
                }

                // 3. Subir el nuevo archivo
                using var ms = new MemoryStream();
                await request.File.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                var options = new Supabase.Storage.FileOptions { Upsert = true };
                await bucket.Upload(fileBytes, filePath, options);

                var url = bucket.GetPublicUrl(filePath);

                // 4. Insertar registro en la tabla UploadedDocuments
                var model2 = _storage.From<UploadedDocumentModel>();
                var newDocument = new UploadedDocumentModel
                {
                    CreditFormId = creditFormId,
                    FileUrl = url
                };

                await model2.Insert(newDocument);

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

            var bucket = _storage.Storage.From("credit-documents");

            var folderPath = $"{creditFormId}/annualFinancials/";
            var fileName = Path.GetFileName(request.File.FileName).Replace(" ", "_");
            var filePath = folderPath + fileName;

            try
            {
                // 1. Eliminar archivos existentes en la carpeta (para mantener solo 1)
                var existingFiles = await bucket.List(folderPath);
                if (existingFiles != null && existingFiles.Count > 0)
                {
                    var deletePaths = existingFiles.Select(f => folderPath + f.Name)
                        .ToList();
                    await bucket.Remove(deletePaths);
                }
                //2. Eliminar registros existentes en la tabla UploadedDocuments
                var model = _storage.From<UploadedDocumentModel>();
                var existingRecord = await model
                    .Where(x => x.CreditFormId == creditFormId && x.FileUrl.Contains($"/{creditFormId}/annualFinancials/"))
                    .Get();
                if (existingRecord.Models.Count > 0)
                {
                    foreach (var record in existingRecord.Models)
                    {
                        await model.Delete(record);
                    }
                }

                // 3. Subir el nuevo archivo
                using var ms = new MemoryStream();
                await request.File.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                var options = new Supabase.Storage.FileOptions { Upsert = true };
                await bucket.Upload(fileBytes, filePath, options);

                var url = bucket.GetPublicUrl(filePath);

                // 4. Insertar registro en la tabla UploadedDocuments
                var model2 = _storage.From<UploadedDocumentModel>();
                var newDocument = new UploadedDocumentModel
                {
                    CreditFormId = creditFormId,
                    FileUrl = url
                };

                await model2.Insert(newDocument);

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

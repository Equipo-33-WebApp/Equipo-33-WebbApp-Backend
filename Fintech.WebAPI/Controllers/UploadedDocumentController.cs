using Fintech.Infrastructure.Persistence.Models;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Authorization;
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
        /// Sube un archivo al servidor.
        /// </summary>
        /// <param name="request">El form-data a subir menor a 2mb, un campo es el archivo y el otro el folder que es el id de un credit-form</param>
        /// <returns>Archivo subido.</returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] DocumentAddRequest request)
        {
            var bucket = _storage.Storage.From("credit-documents");
            var file = request.File;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var filePath = request.CreditFormId + "/" + Path.GetFileName(file.FileName).Replace(" ", "_");

            var options = new Supabase.Storage.FileOptions { Upsert = true };

            await bucket.Upload(fileBytes, filePath, options);

            var url = bucket.GetPublicUrl(filePath);

            // Insertar en la tabla UploadedDocuments
            var model = _storage.From<UploadedDocumentModel>();
            var newDocument = new UploadedDocumentModel
            {
                CreditFormId = request.CreditFormId,
                FileUrl = url,
            };
            await model.Insert(newDocument);

            return Ok(new { FilePath = filePath, Url = url });
           
        }
    }

    public class DocumentAddRequest
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public Guid CreditFormId{ get; set; } // Para crear el folder en supbase
    }
}

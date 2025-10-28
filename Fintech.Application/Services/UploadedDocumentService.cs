using Fintech.Application.Interfaces.UploadedDocuments;
using Fintech.Domain.Entities;

namespace Fintech.Application.Services
{
    public class UploadedDocumentService : IUploadedDocumentService
    {
        private readonly IUploadedDocumentRepository _uploadedDocumentRepository;
        public UploadedDocumentService(IUploadedDocumentRepository uploadedDocumentRepository)
        {
            _uploadedDocumentRepository = uploadedDocumentRepository;
        }

        public async Task<UploadedDocument> AddAsync(Guid creditFormId, string FileUrl)
        {
            var uploadedDocument = new UploadedDocument
            {
                CreditFormId = creditFormId,
                FileUrl = FileUrl,
            };
            await _uploadedDocumentRepository.AddAsync(uploadedDocument);
            return uploadedDocument;
        }

        public async Task DeleteUploadedDocumentRecordsAsync(Guid creditFormId, string filePathContains)
        {
            await _uploadedDocumentRepository.DeleteUploadedDocumentRecordsAsync(creditFormId, filePathContains);
        }
    }
}
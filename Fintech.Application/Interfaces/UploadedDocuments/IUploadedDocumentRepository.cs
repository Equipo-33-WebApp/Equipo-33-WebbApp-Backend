using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.UploadedDocuments
{
    public interface IUploadedDocumentRepository
    {
        Task<UploadedDocument> AddAsync(UploadedDocument uploadedDocument);
        Task DeleteUploadedDocumentRecordsAsync(Guid creditFormId, string filePathContains);
    }
}
using Fintech.Domain.Entities;

namespace Fintech.Application.Interfaces.UploadedDocuments
{
    public interface IUploadedDocumentService
    {
        Task DeleteUploadedDocumentRecordsAsync(Guid creditFormId, string filePathContains);
        Task<UploadedDocument> AddAsync(Guid CreditFormId, string FileUrl);
    }
}
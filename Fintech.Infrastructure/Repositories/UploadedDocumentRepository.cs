using AutoMapper;
using Fintech.Application.Interfaces.UploadedDocuments;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using Supabase;

namespace Fintech.Infrastructure.Repositories
{
    public class UploadedDocumentRepository : IUploadedDocumentRepository
    {
        private readonly Client _supabaseClient;
        private readonly IMapper _mapper;

        public UploadedDocumentRepository(Client client, IMapper mapper)
        {
            _supabaseClient = client;
            _mapper = mapper;
        }
        public async Task<UploadedDocument> AddAsync(UploadedDocument uploadedDocument)
        {
            var model = _supabaseClient.From<UploadedDocumentModel>();

            var uploadedDocumentModel = _mapper.Map<UploadedDocumentModel>(uploadedDocument);

            await model.Insert(uploadedDocumentModel);

            return uploadedDocument;
        }

        public async Task DeleteUploadedDocumentRecordsAsync(Guid creditFormId, string filePathContains)
        {
            // Eliminar registros existentes en la tabla UploadedDocuments
            var model = _supabaseClient.From<UploadedDocumentModel>();
            var existingRecord = await model
                .Where(x => x.CreditFormId == creditFormId && x.FileUrl.Contains(filePathContains))
                .Get();

            if (existingRecord.Models.Count > 0)
            {
                foreach (var record in existingRecord.Models)
                {
                    await model.Delete(record);
                }
            }
        }
    }
}
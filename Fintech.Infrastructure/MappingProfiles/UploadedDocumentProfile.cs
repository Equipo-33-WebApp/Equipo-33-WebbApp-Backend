using AutoMapper;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles
{
    public class UploadedDocumentProfile : Profile
    {
        public UploadedDocumentProfile()
        {
            CreateMap<UploadedDocument, UploadedDocumentModel>().ReverseMap();
        }
    }
}
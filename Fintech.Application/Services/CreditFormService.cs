using AutoMapper;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Application.Services
{
    public class CreditFormService : ICreditFormService
    {
        private readonly ICreditFormRepository _creditFormRepository;
        private readonly IMapper _mapper;
        public CreditFormService(ICreditFormRepository creditFormRepository, IMapper mapper)
        {
            _creditFormRepository = creditFormRepository;
            _mapper = mapper;
        }
        public async Task<CreditForm?> GetByIdAsync(Guid id)
        {
            return await _creditFormRepository.GetByIdAsync(id);
        }
    }
}

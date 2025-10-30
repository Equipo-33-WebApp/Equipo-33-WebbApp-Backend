using AutoMapper;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;
using Fintech.Infrastructure.Persistence.Models;
using Fintech.Infrastructure.Persistence.Models.Panel;
using GenerativeAI.Types.RagEngine;
using Supabase;

namespace Fintech.Infrastructure.Repositories;

public class PanelRepository(Client _client, IMapper _mapper) : IPanelRepository
{
    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(CreditFilter filter)
    {
        var from = (filter.Page - 1) * filter.PageSize;
        var to = from + filter.PageSize - 1;

        var table = _client.From<CreditApplicationPanelModel>();

        var filtered = !string.IsNullOrWhiteSpace(filter.Status)
            ? table.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, filter.Status)
            : table;

        var result = await filtered
            .Order(x => x.UpdatedAt, Supabase.Postgrest.Constants.Ordering.Descending)
            .Range(from, to)
            .Get();

        var models = result.Models;
        
        return _mapper.Map<IEnumerable<CreditApplicationPanel>>(models);
    }

    public async Task<CreditForm?> UpdateStatusAsync(Guid creditFormId, string newStatus)
    {
        var creditForm = await _client
            .From<CreditFormModel>()
            .Where(cf => cf.Id == creditFormId)
            .Set(cf => new KeyValuePair<object, object?>(cf.Status, newStatus))
            .Update();

        return _mapper.Map<CreditForm>(creditForm.Models.First());
    }

    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationByPymeAsync(Guid pymeId, CreditFilter filter)
    {
        var from = (filter.Page - 1) * filter.PageSize;
        var to = from + filter.PageSize - 1;

        var table = _client.From<CreditApplicationPanelModel>();

        var filtered = table.Filter("pyme_id", Supabase.Postgrest.Constants.Operator.Equals, pymeId.ToString());

        filtered = !string.IsNullOrWhiteSpace(filter.Status)
            ? table.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, filter.Status)
            : table;

        var result = await filtered
            .Order(x => x.UpdatedAt, Supabase.Postgrest.Constants.Ordering.Descending)
            .Range(from, to)
            .Get();

        var models = result.Models;

        return _mapper.Map<IEnumerable<CreditApplicationPanel>>(models);
    }
}
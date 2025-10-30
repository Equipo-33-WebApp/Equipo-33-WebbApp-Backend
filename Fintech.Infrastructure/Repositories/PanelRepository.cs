using AutoMapper;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;
using Fintech.Infrastructure.Persistence.Models;
using Fintech.Infrastructure.Persistence.Models.Panel;
using Supabase;

namespace Fintech.Infrastructure.Repositories;

public class PanelRepository(Client _client, IMapper _mapper) : IPanelRepository
{
    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationAsync(int page, int pageSize, string status)
    {
        var from = (page - 1) * pageSize;
        var to = from + pageSize - 1;

        var table = _client.From<CreditApplicationPanelModel>();

        var filtered = !string.IsNullOrWhiteSpace(status)
            ? table.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, status)
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

    public async Task<IEnumerable<CreditApplicationPanel>> GetAllCreditApplicationByPymeAsync(Guid pymeId, int page, int pageSize, string status)
    {
        var from = (page - 1) * pageSize;
        var to = from + pageSize - 1;

        var table = _client.From<CreditApplicationPanelModel>();

        var filtered = table.Filter("pyme_id", Supabase.Postgrest.Constants.Operator.Equals, pymeId.ToString());

        filtered = !string.IsNullOrWhiteSpace(status)
            ? table.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, status)
            : table;

        var result = await filtered
            .Order(x => x.UpdatedAt, Supabase.Postgrest.Constants.Ordering.Descending)
            .Range(from, to)
            .Get();

        var models = result.Models;

        return _mapper.Map<IEnumerable<CreditApplicationPanel>>(models);
    }
}
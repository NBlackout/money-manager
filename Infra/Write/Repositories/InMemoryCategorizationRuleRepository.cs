using App.Write.Model.CategorizationRules;
using App.Write.Ports;

namespace Infra.Write.Repositories;

public class InMemoryCategorizationRuleRepository : ICategorizationRuleRepository
{
    private readonly Dictionary<CategorizationRuleId, CategorizationRuleSnapshot> data = new();

    public IEnumerable<CategorizationRuleSnapshot> Data => this.data.Values.Select(c => c);
    public Func<CategorizationRuleId> NextId { get; set; } = null!;

    public Task<CategorizationRuleId> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<CategorizationRule> By(CategorizationRuleId id) =>
        Task.FromResult(new CategorizationRule(this.data[id]));

    public Task Save(CategorizationRule categorizationRule)
    {
        this.data[categorizationRule.Id] = categorizationRule.Snapshot;

        return Task.CompletedTask;
    }

    public Task Delete(CategorizationRuleId id)
    {
        this.data.Remove(id);

        return Task.CompletedTask;
    }

    public void Feed(params CategorizationRuleSnapshot[] categories) =>
        categories.ToList().ForEach(categorizationRule => this.data[categorizationRule.Id] = categorizationRule);
}
namespace Write.Infra.Repositories;

public class InMemoryBankRepository : IBankRepository
{
    private readonly Dictionary<Guid, Bank> data = new();
    private readonly Dictionary<string, Bank> dataByExternalId = new();

    public IEnumerable<BankSnapshot> Data => this.data.Values.Select(b => b.Snapshot);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Bank> ById(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task<Bank?> ByExternalIdOrDefault(string externalId) =>
        Task.FromResult(this.dataByExternalId.TryGetValue(externalId, out Bank? bank) ? bank : null);

    public Task Save(Bank bank)
    {
        BankSnapshot snapshot = bank.Snapshot;
        this.data[bank.Id] = Bank.From(snapshot);
        this.dataByExternalId[snapshot.ExternalId] = Bank.From(snapshot);

        return Task.CompletedTask;
    }

    public void FeedByExternalId(string externalId, Bank bank) =>
        this.dataByExternalId.Add(externalId, bank);

    public void Feed(params Bank[] banks) =>
        banks.ToList().ForEach(bank => this.data[bank.Id] = bank);

    public void Clear()
    {
        this.data.Clear();
        this.dataByExternalId.Clear();
    }
}
namespace Write.Infra.Repositories;

public class InMemoryBankRepository : IBankRepository
{
    private readonly Dictionary<Guid, BankSnapshot> data = new();
    private readonly Dictionary<string, BankSnapshot> dataByExternalId = new();

    public IEnumerable<BankSnapshot> Data => this.data.Values.Select(b => b);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Bank> By(Guid id) =>
        Task.FromResult(Bank.From(this.data[id]));

    public Task<Bank?> ByExternalIdOrDefault(string externalId) =>
        Task.FromResult(this.dataByExternalId.TryGetValue(externalId, out BankSnapshot? value) ? Bank.From(value) : null);

    public Task Save(Bank bank)
    {
        BankSnapshot snapshot = bank.Snapshot;
        this.data[bank.Id] = snapshot;
        this.dataByExternalId[snapshot.ExternalId] = snapshot;

        return Task.CompletedTask;
    }

    public void FeedByExternalId(string externalId, Bank bank) =>
        this.dataByExternalId.Add(externalId, bank.Snapshot);

    public void Feed(params Bank[] banks) =>
        banks.ToList().ForEach(bank => this.data[bank.Id] = bank.Snapshot);

    public void Clear()
    {
        this.data.Clear();
        this.dataByExternalId.Clear();
    }
}
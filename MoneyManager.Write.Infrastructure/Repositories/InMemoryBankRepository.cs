namespace MoneyManager.Write.Infrastructure.Repositories;

public class InMemoryBankRepository : IBankRepository
{
    private readonly Dictionary<Guid, Bank> data = new();
    private readonly Dictionary<string, Bank> dataByExternalId = new();

    public Dictionary<Guid, Bank> Data => this.data;
    public Func<Guid> NextId { get; set; } = Guid.NewGuid;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Bank> GetById(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task<Bank?> GetByExternalIdOrDefault(string externalId) =>
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
        banks.ToList().ForEach(bank => this.data.Add(bank.Id, bank));

    public void Clear()
    {
        this.data.Clear();
        this.dataByExternalId.Clear();
    }
}
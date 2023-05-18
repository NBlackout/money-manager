namespace MoneyManager.Write.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, Account> data = new();
    private readonly Dictionary<ExternalId, Account> dataByExternalId = new();

    public IEnumerable<AccountSnapshot> Data => this.data.Values.Select(a => a.Snapshot);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Account> ById(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task<Account?> ByExternalIdOrDefault(ExternalId externalId) =>
        Task.FromResult(this.dataByExternalId.TryGetValue(externalId, out Account? account) ? account : null);

    public Task Save(Account account)
    {
        AccountSnapshot snapshot = account.Snapshot;
        this.data[account.Id] = Account.From(snapshot);
        this.dataByExternalId[new ExternalId(snapshot.BankId, snapshot.Number)] = Account.From(snapshot);

        return Task.CompletedTask;
    }

    public void FeedByExternalId(ExternalId externalId, Account account) =>
        this.dataByExternalId.Add(externalId, account);

    public void Feed(params Account[] accounts) =>
        accounts.ToList().ForEach(account => this.data[account.Id] = account);

    public void Clear()
    {
        this.data.Clear();
        this.dataByExternalId.Clear();
    }
}
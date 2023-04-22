namespace MoneyManager.Infrastructure.Write.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, AccountSnapshot> data = new();
    private readonly Dictionary<ExternalId, AccountSnapshot> dataByExternalId = new();

    public IEnumerable<AccountSnapshot> Data => this.data.Values;
    public Func<Guid> NextId { get; set; } = Guid.NewGuid;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Account> GetById(Guid id) =>
        Task.FromResult(Account.From(this.data[id]));

    public Task<Account?> GetByExternalIdOrDefault(ExternalId externalId) =>
        Task.FromResult(this.dataByExternalId.ContainsKey(externalId) ? Account.From(this.dataByExternalId[externalId]) : null);

    public Task Save(Account account)
    {
        this.data[account.Id] = account.Snapshot;

        return Task.CompletedTask;
    }

    public void FeedByExternalId(ExternalId externalId, AccountSnapshot account) =>
        this.dataByExternalId.Add(externalId, account);

    public void Feed(params AccountSnapshot[] accounts) =>
        accounts.ToList().ForEach(account => this.data.Add(account.Id, account));

    public void Clear()
    {
        this.data.Clear();
        this.dataByExternalId.Clear();
    }
}
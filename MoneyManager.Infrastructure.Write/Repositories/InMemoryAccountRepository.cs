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

    public Task<Account?> GetByExternalIdOrDefault(ExternalId id) =>
        Task.FromResult(this.dataByExternalId.ContainsKey(id) ? Account.From(this.dataByExternalId[id]) : null);

    public Task Save(Account account)
    {
        this.data[account.Id] = account.Snapshot;

        return Task.CompletedTask;
    }

    public void FeedByExternalId(ExternalId externalId, Account account) =>
        this.dataByExternalId.Add(externalId, account.Snapshot);

    public void Feed(params Account[] accounts) =>
        accounts.ToList().ForEach(account => this.data.Add(account.Id, account.Snapshot));
}
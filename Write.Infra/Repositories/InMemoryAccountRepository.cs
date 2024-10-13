namespace Write.Infra.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, AccountSnapshot> data = new();
    private readonly Dictionary<string, AccountSnapshot> dataByExternalId = new();

    public IEnumerable<AccountSnapshot> Data => this.data.Values.Select(a => a);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Account> By(Guid id) =>
        Task.FromResult(Account.From(this.data[id]));

    public Task<Account?> ByOrDefault(string externalId) =>
        Task.FromResult(this.dataByExternalId.TryGetValue(externalId, out AccountSnapshot? value)
            ? Account.From(value)
            : null);

    public Task Save(Account account)
    {
        AccountSnapshot snapshot = account.Snapshot;
        this.data[account.Id] = snapshot;
        this.dataByExternalId[snapshot.Number] = snapshot;

        return Task.CompletedTask;
    }

    public void Feed(params AccountSnapshot[] accounts) =>
        accounts.ToList().ForEach(account => this.data[account.Id] = account);

    public void FeedByExternalId(string externalId, AccountSnapshot account) =>
        this.dataByExternalId.Add(externalId, account);
}
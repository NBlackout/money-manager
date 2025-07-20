using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace Infra.Write.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountId, AccountSnapshot> data = new();
    private readonly Dictionary<string, AccountSnapshot> dataByExternalId = new();

    public IEnumerable<AccountSnapshot> Data => this.data.Values.Select(a => a);
    public Func<AccountId> NextId { get; set; } = null!;

    public Task<AccountId> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Account> By(AccountId id) =>
        Task.FromResult(Account.From(this.data[id]));

    public Task<Account?> ByOrDefault(ExternalId externalId) =>
        Task.FromResult(this.dataByExternalId.TryGetValue(externalId.Value, out AccountSnapshot? value) ? Account.From(value) : null);

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
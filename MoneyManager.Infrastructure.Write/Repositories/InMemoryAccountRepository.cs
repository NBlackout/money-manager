namespace MoneyManager.Infrastructure.Write.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<ExternalId, Account> data = new();

    public IEnumerable<Account> Data => this.data.Values;
    public Func<Guid> NextId { get; set; } = Guid.NewGuid;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Account?> GetByIdOrDefault(ExternalId id) =>
        Task.FromResult(this.data.ContainsKey(id) ? this.data[id] : null);

    public Task Save(Account account)
    {
        this.data[account.ExternalId] = account;

        return Task.CompletedTask;
    }

    public void Feed(params Account[] accounts) =>
        accounts.ToList().ForEach(account => this.data.Add(account.ExternalId, account));
}
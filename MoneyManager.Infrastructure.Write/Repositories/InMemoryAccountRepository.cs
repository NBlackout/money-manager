namespace MoneyManager.Infrastructure.Write.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountId, Account> data = new();

    public Task<Account> GetById(AccountId id) =>
        Task.FromResult(this.data[id]);

    public Task<bool> Exists(AccountId id) =>
        Task.FromResult(this.data.ContainsKey(id));

    public Task Save(Account account)
    {
        this.Feed(account);

        return Task.CompletedTask;
    }

    public void Feed(Account account) => 
        this.data.Add(account.Id, account);
}
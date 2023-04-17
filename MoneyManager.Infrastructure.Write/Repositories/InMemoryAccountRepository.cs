namespace MoneyManager.Infrastructure.Write.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, Account> accounts = new();
    private readonly List< string> existingNumbers = new();
    private Guid nextIdentity;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.nextIdentity);

    public Task<Account> GetById(Guid id) =>
        Task.FromResult(this.accounts[id]);

    public Task<bool> ExistsByNumber(string number) =>
        Task.FromResult(this.existingNumbers.Contains(number));

    public Task Save(Account account)
    {
        this.Feed(account);
        this.existingNumbers.Add(account.Number);

        return Task.CompletedTask;
    }

    public void SetNextIdentity(Guid id) =>
        this.nextIdentity = id;

    public void Feed(Account account) =>
        this.accounts.Add(account.Id, account);

    public void MarkAsExisting(string number) =>
        this.existingNumbers.Add(number);
}
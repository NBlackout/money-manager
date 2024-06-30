namespace Write.App.Ports;

public interface IAccountRepository
{
    Task<Guid> NextIdentity();
    Task<Account> By(Guid id);
    Task<Account?> ByOrDefault(string externalId);
    Task Save(Account account);
}

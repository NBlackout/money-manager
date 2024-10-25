namespace Write.App.Ports;

public interface IAccountRepository
{
    Task<AccountId> NextIdentity();
    Task<Account> By(AccountId id);
    Task<Account?> ByOrDefault(string externalId);
    Task Save(Account account);
}

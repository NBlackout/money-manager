namespace Write.App.Ports;

public interface IAccountRepository
{
    Task<Guid> NextIdentity();
    Task<Account> By(Guid id);
    Task<Account?> ByExternalIdOrDefault(ExternalId externalId);
    Task Save(Account account);
}
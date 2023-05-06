namespace MoneyManager.Write.Application.Ports;

public interface IAccountRepository
{
    Task<Guid> NextIdentity();
    Task<Account> ById(Guid id);
    Task<Account?> ByExternalIdOrDefault(ExternalId externalId);
    Task Save(Account account);
}
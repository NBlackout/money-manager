namespace MoneyManager.Write.Application.Ports;

public interface IAccountRepository
{
    Task<Guid> NextIdentity();
    Task<Account> GetById(Guid id);
    Task<Account?> GetByExternalIdOrDefault(ExternalId externalId);
    Task Save(Account account);
}
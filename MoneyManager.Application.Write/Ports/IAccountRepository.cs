namespace MoneyManager.Application.Write.Ports;

public interface IAccountRepository
{
    Task<Guid> NextIdentity();
    Task<Account?> GetByExternalIdOrDefault(ExternalId id);
    Task Save(Account account);
}
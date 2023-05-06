namespace MoneyManager.Write.Application.Ports;

public interface IBankRepository
{
    Task<Guid> NextIdentity();
    Task<Bank> ById(Guid id);
    Task<Bank?> ByExternalIdOrDefault(string externalId);
    Task Save(Bank bank);
}
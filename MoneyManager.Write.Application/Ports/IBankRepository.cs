namespace MoneyManager.Write.Application.Ports;

public interface IBankRepository
{
    Task<Guid> NextIdentity();
    Task<Bank> GetById(Guid id);
    Task<Bank?> GetByExternalIdOrDefault(string externalId);
    Task Save(Bank bank);
}
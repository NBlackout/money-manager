namespace Write.App.Ports;

public interface IBankRepository
{
    Task<Guid> NextIdentity();
    Task<Bank> By(Guid id);
    Task<Bank?> ByExternalIdOrDefault(string externalId);
    Task Save(Bank bank);
}
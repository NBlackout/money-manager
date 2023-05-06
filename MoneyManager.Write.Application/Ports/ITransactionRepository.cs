namespace MoneyManager.Write.Application.Ports;

public interface ITransactionRepository
{
    Task<Guid> NextIdentity();
    Task<IReadOnlyCollection<string>> UnknownExternalIds(IEnumerable<string> externalIds);
    Task Save(Transaction transaction);
}
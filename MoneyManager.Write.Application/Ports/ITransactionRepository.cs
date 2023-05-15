namespace MoneyManager.Write.Application.Ports;

public interface ITransactionRepository
{
    Task<Guid> NextIdentity();
    Task<Transaction> ById(Guid id);
    Task<IReadOnlyCollection<string>> UnknownExternalIds(IEnumerable<string> externalIds);
    Task Save(Transaction transaction);
}
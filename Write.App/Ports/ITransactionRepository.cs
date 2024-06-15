namespace Write.App.Ports;

public interface ITransactionRepository
{
    Task<Guid> NextIdentity();
    Task<Transaction> By(Guid id);
    Task<string[]> UnknownExternalIds(IEnumerable<string> externalIds);
    Task Save(Transaction transaction);
}
namespace Write.App.Ports;

public interface ITransactionRepository
{
    Task<TransactionId> NextIdentity();
    Task<Transaction> By(TransactionId id);
    Task<string[]> UnknownExternalIds(IEnumerable<string> externalIds);
    Task Save(Transaction transaction);
}
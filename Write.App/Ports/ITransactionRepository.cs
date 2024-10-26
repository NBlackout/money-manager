namespace Write.App.Ports;

public interface ITransactionRepository
{
    Task<TransactionId> NextIdentity();
    Task<Transaction> By(TransactionId id);
    Task<ExternalId[]> UnknownExternalIds(IEnumerable<ExternalId> externalIds);
    Task Save(Transaction transaction);
}
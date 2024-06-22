namespace Read.App.Ports;

public interface ITransactionsToCategorizeDataSource
{
    Task<TransactionToCategorize[]> Get();
}

public record TransactionToCategorize(Guid Id, string Label, decimal Amount);
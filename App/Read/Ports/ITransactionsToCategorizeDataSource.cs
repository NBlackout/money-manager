namespace App.Read.Ports;

public interface ITransactionsToCategorizeDataSource
{
    Task<TransactionToCategorize[]> All();
}

public record TransactionToCategorize(Guid Id, string Label, decimal Amount);
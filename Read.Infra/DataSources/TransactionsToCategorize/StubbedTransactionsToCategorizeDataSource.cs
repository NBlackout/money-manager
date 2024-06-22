namespace Read.Infra.DataSources.TransactionsToCategorize;

public class StubbedTransactionsToCategorizeDataSource : ITransactionsToCategorizeDataSource
{
    private readonly List<TransactionToCategorize> transactionsToCategorize = [];

    public Task<TransactionToCategorize[]> Get() =>
        Task.FromResult(this.transactionsToCategorize.ToArray());

    public void Feed(params TransactionToCategorize[] transactionsToCategorize) =>
        this.transactionsToCategorize.AddRange(transactionsToCategorize);
}
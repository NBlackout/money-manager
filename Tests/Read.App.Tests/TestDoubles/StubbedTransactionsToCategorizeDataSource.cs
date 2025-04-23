namespace Read.App.Tests.TestDoubles;

public class StubbedTransactionsToCategorizeDataSource : ITransactionsToCategorizeDataSource
{
    private readonly List<TransactionToCategorize> transactionsToCategorize = [];

    public Task<TransactionToCategorize[]> All() =>
        Task.FromResult(this.transactionsToCategorize.ToArray());

    public void Feed(params TransactionToCategorize[] transactionsToCategorize) =>
        this.transactionsToCategorize.AddRange(transactionsToCategorize);
}
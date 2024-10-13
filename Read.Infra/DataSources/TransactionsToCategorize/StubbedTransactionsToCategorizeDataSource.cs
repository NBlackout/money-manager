﻿namespace Read.Infra.DataSources.TransactionsToCategorize;

public class StubbedTransactionsToCategorizeDataSource : ITransactionsToCategorizeDataSource
{
    private readonly List<TransactionToCategorize> transactionsToCategorize = [];

    public Task<TransactionToCategorize[]> All() =>
        Task.FromResult(this.transactionsToCategorize.ToArray());

    public void Feed(params TransactionToCategorize[] expected) =>
        this.transactionsToCategorize.AddRange(expected);
}
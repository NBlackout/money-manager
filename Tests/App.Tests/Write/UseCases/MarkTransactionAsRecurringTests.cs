using App.Tests.Write.Tooling;
using App.Write.Model.RecurringTransactions;
using App.Write.Model.Transactions;
using App.Write.UseCases;
using Infra.Write.Repositories;
using static App.Tests.Write.Tooling.SnapshotHelpers;

namespace App.Tests.Write.UseCases;

public class MarkTransactionAsRecurringTests
{
    private static readonly RecurringTransactionId TheRecurringTransactionId = Any<RecurringTransactionId>();

    private readonly InMemoryTransactionRepository transactionRepository = new();
    private readonly InMemoryRecurringTransactionRepository recurringTransactionRepository = new();
    private readonly MarkTransactionAsRecurring sut;

    public MarkTransactionAsRecurringTests()
    {
        this.sut = new MarkTransactionAsRecurring(this.transactionRepository, this.recurringTransactionRepository);
    }

    [Fact]
    public async Task Marks_as_recurring()
    {
        TransactionSnapshot transaction = ATransaction() with { Date = DateOnly.Parse("2025-01-01") };
        this.Feed(transaction);
        await this.Verify(ExpectedTransactionFrom(transaction), ExpectedRecurringTransactionFrom(transaction, DateOnly.Parse("2025-02-01")));
    }

    [Fact]
    public async Task Adjusts_to_last_day_of_next_month()
    {
        TransactionSnapshot transaction = ATransaction() with { Date = DateOnly.Parse("2025-01-31") };
        this.Feed(transaction);
        await this.Verify(ExpectedTransactionFrom(transaction), ExpectedRecurringTransactionFrom(transaction, DateOnly.Parse("2025-02-28")));
    }

    [Fact]
    public async Task Tells_when_already_marked_as_recurring()
    {
        TransactionSnapshot transaction = ATransaction() with { IsRecurring = true };
        this.Feed(transaction);
        await this.Verify<TransactionAlreadyMarkedAsRecurringException>(transaction);
    }

    private async Task Verify(TransactionSnapshot expectedTransaction, RecurringTransactionSnapshot expectedRecurringTransaction)
    {
        await this.sut.Execute(expectedTransaction.Id, expectedRecurringTransaction.Id);

        Transaction actualTransaction = await this.transactionRepository.By(expectedTransaction.Id);
        actualTransaction.Snapshot.Should().Be(expectedTransaction);

        RecurringTransaction actualRecurringTransaction = await this.recurringTransactionRepository.By(expectedRecurringTransaction.Id);
        actualRecurringTransaction.Snapshot.Should().Be(expectedRecurringTransaction);
    }

    private async Task Verify<TException>(TransactionSnapshot transaction) where TException : Exception =>
        await this.Invoking(s => s.Verify(transaction, ARecurringTransaction())).Should().ThrowAsync<TException>();

    private void Feed(TransactionSnapshot transaction) =>
        this.transactionRepository.Feed(transaction);

    private static TransactionSnapshot ExpectedTransactionFrom(TransactionSnapshot transaction) =>
        transaction with { IsRecurring = true };

    private static RecurringTransactionSnapshot ExpectedRecurringTransactionFrom(TransactionSnapshot transaction, DateOnly date) =>
        new(TheRecurringTransactionId, transaction.Amount, transaction.Label, date, transaction.CategoryId);

    private static TransactionSnapshot ATransaction() =>
        SnapshotHelpers.ATransaction() with { IsRecurring = false };
}
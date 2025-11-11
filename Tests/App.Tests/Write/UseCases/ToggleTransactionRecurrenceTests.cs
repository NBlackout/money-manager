using App.Write.Model.Transactions;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class ToggleTransactionRecurrenceTests
{
    private readonly InMemoryTransactionRepository repository = new();
    private readonly ToggleTransactionRecurrence sut;

    public ToggleTransactionRecurrenceTests()
    {
        this.sut = new ToggleTransactionRecurrence(this.repository);
    }

    [Theory]
    [InlineRandomData(true, false)]
    [InlineRandomData(false, true)]
    public async Task Toggles_recurrence(bool wasRecurring, bool isNowRecurring, TransactionSnapshot transaction)
    {
        this.Feed(transaction with { IsRecurring = wasRecurring });
        await this.Verify(transaction with { IsRecurring = isNowRecurring });
    }

    private async Task Verify(TransactionSnapshot expected)
    {
        await this.sut.Execute(expected.Id);

        Transaction actual = await this.repository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }

    private void Feed(TransactionSnapshot transaction) =>
        this.repository.Feed(transaction);
}
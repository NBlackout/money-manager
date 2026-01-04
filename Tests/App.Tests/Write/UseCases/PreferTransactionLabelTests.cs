using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;
using App.Write.UseCases.Transactions;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class PreferTransactionLabelTests
{
    private readonly InMemoryTransactionRepository repository = new();
    private readonly PreferTransactionLabel sut;

    public PreferTransactionLabelTests()
    {
        this.sut = new PreferTransactionLabel(this.repository);
    }

    [Theory, RandomData]
    public async Task Prefers_transaction_label(TransactionSnapshot transaction, Label label)
    {
        this.Feed(transaction);
        await this.Verify(label, transaction with { PreferredLabel = label.Value });
    }

    private async Task Verify(Label label, TransactionSnapshot expected)
    {
        await this.sut.Execute(expected.Id, label);

        Transaction actual = await this.repository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }

    private void Feed(TransactionSnapshot transaction) =>
        this.repository.Feed(transaction);
}
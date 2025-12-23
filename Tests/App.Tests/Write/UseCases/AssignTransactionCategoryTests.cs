using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class AssignTransactionCategoryTests
{
    private readonly InMemoryTransactionRepository repository = new();
    private readonly AssignTransactionCategory sut;

    public AssignTransactionCategoryTests()
    {
        this.sut = new AssignTransactionCategory(this.repository);
    }

    [Theory, RandomData]
    public async Task Assigns_transaction_category(TransactionSnapshot transaction, CategoryId categoryId)
    {
        this.Feed(transaction);
        await this.Verify(categoryId, transaction with { CategoryId = categoryId });
    }

    private async Task Verify(CategoryId categoryId, TransactionSnapshot expected)
    {
        await this.sut.Execute(expected.Id, categoryId);

        Transaction actual = await this.repository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }

    private void Feed(TransactionSnapshot transaction) =>
        this.repository.Feed(transaction);
}
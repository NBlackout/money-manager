using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.UseCases.Categories;
using Infra.Write.Repositories;
using static App.Tests.Write.Tooling.SnapshotHelpers;

namespace App.Tests.Write.UseCases;

public class DeleteCategoryTests
{
    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly InMemoryTransactionRepository transactionRepository = new();
    private readonly DeleteCategory sut;

    public DeleteCategoryTests()
    {
        this.sut = new DeleteCategory(this.categoryRepository, this.transactionRepository);
    }

    [Theory, RandomData]
    public async Task Deletes_category(CategorySnapshot category)
    {
        this.Feed(category);
        await this.Verify(category);
    }

    [Theory, RandomData]
    public async Task Clears_transactions_category(CategorySnapshot category)
    {
        TransactionSnapshot aTransaction = ATransaction() with { CategoryId = category.Id };
        TransactionSnapshot anotherTransaction = ATransaction() with { CategoryId = category.Id };
        this.Feed(category, aTransaction, anotherTransaction);

        await this.Verify(category, aTransaction with { CategoryId = null }, anotherTransaction with { CategoryId = null });
    }

    private async Task Verify(CategorySnapshot expectedCategory, params TransactionSnapshot[] expectedTransactions)
    {
        await this.sut.Execute(expectedCategory.Id);

        bool exists = this.categoryRepository.Exists(expectedCategory.Id);
        exists.Should().BeFalse();

        TransactionSnapshot[] transactions = this.transactionRepository.Data.ToArray();
        transactions.Should().Equal(expectedTransactions);
    }

    private void Feed(CategorySnapshot category, params TransactionSnapshot[] transactions)
    {
        this.categoryRepository.Feed(category);
        this.transactionRepository.Feed(transactions);
    }
}
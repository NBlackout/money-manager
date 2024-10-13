namespace Write.App.Tests.UseCases;

public class AssignTransactionCategoryTests
{
    private readonly InMemoryTransactionRepository repository = new();
    private readonly AssignTransactionCategory sut;

    public AssignTransactionCategoryTests()
    {
        this.sut = new AssignTransactionCategory(this.repository);
    }

    [Theory, RandomData]
    public async Task Assigns_transaction_category(TransactionSnapshot transaction, Guid categoryId)
    {
        this.Feed(transaction);
        await this.Verify(transaction, categoryId);
    }

    private async Task Verify(TransactionSnapshot transaction, Guid categoryId)
    {
        await this.sut.Execute(transaction.Id, categoryId);

        Transaction actual = await this.repository.By(transaction.Id);
        actual.Snapshot.Should().Be(transaction with { CategoryId = categoryId });
    }

    private void Feed(TransactionSnapshot transaction) =>
        this.repository.Feed(transaction);
}
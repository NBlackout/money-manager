namespace MoneyManager.Write.Application.Tests.UseCases;

public class AssignTransactionCategoryTests
{
    private readonly InMemoryTransactionRepository repository;
    private readonly AssignTransactionCategory sut;

    public AssignTransactionCategoryTests()
    {
        this.repository = new InMemoryTransactionRepository();
        this.sut = new AssignTransactionCategory(this.repository);
    }

    [Fact]
    public async Task Should_assign_transaction_category()
    {
        Transaction transaction = TransactionBuilder.For(Guid.NewGuid()).Build();
        Guid categoryId = Guid.NewGuid();
        this.repository.Feed(transaction);

        await this.sut.Execute(transaction.Id, categoryId);

        Transaction actual = await this.repository.ById(transaction.Id);
        actual.Snapshot.Should().Be(transaction.Snapshot with { CategoryId = categoryId });
    }
}
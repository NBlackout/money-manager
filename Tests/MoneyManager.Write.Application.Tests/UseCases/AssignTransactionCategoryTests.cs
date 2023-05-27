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
        Transaction existing = TransactionBuilder.For(Guid.NewGuid()).Build();
        Guid categoryId = Guid.NewGuid();
        this.repository.Feed(existing);

        await this.sut.Execute(existing.Id, categoryId);

        Transaction actual = await this.repository.ById(existing.Id);
        actual.Snapshot.Should().Be(existing.Snapshot with { CategoryId = categoryId });
    }
}
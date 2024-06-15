using static Shared.TestTooling.Randomizer;

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
    public async Task Should_assign_transaction_category(TransactionBuilder transaction)
    {
        this.repository.Feed(transaction.Build());

        Guid categoryId = Another(transaction.CategoryId);
        await this.sut.Execute(transaction.Id, categoryId);

        Transaction actual = await this.repository.By(transaction.Id);
        actual.Snapshot.Should().Be(transaction.Build().Snapshot with { CategoryId = categoryId });
    }
}
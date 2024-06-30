using Write.App.Model.Budgets;
using Write.App.Model.Exceptions;

namespace Write.App.Tests;

public class DefineBudgetTests
{
    private readonly InMemoryBudgetRepository budgetRepository = new();
    private readonly DefineBudget sut;

    public DefineBudgetTests()
    {
        this.sut = new DefineBudget(this.budgetRepository);
    }

    [Theory, RandomData]
    public async Task Defines(BudgetSnapshot newBudget)
    {
        await this.Verify(newBudget);
    }

    [Theory, RandomData]
    public async Task Prevents_duplication_of(BudgetSnapshot budget)
    {
        this.Feed(budget);

        await this.Verify<BudgetAlreadyDefinedException>(budget);
    }

    private async Task Verify(BudgetSnapshot expected)
    {
        await this.sut.Execute(expected.Id, expected.Name, expected.Amount);

        Budget actual = this.budgetRepository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }

    private async Task Verify<TException>(BudgetSnapshot budget) where TException : Exception =>
        await this.Invoking(s => s.Verify(budget)).Should().ThrowAsync<TException>();

    private void Feed(BudgetSnapshot budget) =>
        this.budgetRepository.Feed(budget);
}
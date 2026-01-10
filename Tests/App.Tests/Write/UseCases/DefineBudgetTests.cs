using App.Write.Model.Budgets;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class DefineBudgetTests
{
    private readonly InMemoryBudgetRepository budgetRepository = new();
    private readonly DefineBudget sut;

    public DefineBudgetTests()
    {
        this.sut = new DefineBudget(this.budgetRepository);
    }

    [Theory, RandomData]
    public async Task Defines(BudgetSnapshot budget) =>
        await this.Verify(budget);

    [Theory, RandomData]
    public async Task Prevents_duplication_of(BudgetSnapshot budget)
    {
        this.Feed(budget);

        await this.Verify<BudgetAlreadyDefinedException>(budget);
    }

    private async Task Verify(BudgetSnapshot expected)
    {
        await this.sut.Execute(expected.Id, new Label(expected.Name), new Amount(expected.Amount), expected.BeginDate);

        Budget actual = this.budgetRepository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }

    private async Task Verify<TException>(BudgetSnapshot budget) where TException : Exception =>
        await this.Invoking(s => s.Verify(budget)).Should().ThrowAsync<TException>();

    private void Feed(BudgetSnapshot budget) =>
        this.budgetRepository.Feed(budget);
}
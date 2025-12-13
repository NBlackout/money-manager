using App.Read.Ports;
using App.Shared.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TestTooling.TestDoubles;

namespace Infra.Tests.Read.DataSources;

public class InMemoryBudgetSummariesDataSourceTests : InfraTest<IBudgetSummariesDataSource, InMemoryBudgetSummariesDataSource>
{
    private readonly InMemoryBudgetRepository budgetRepository;
    private readonly StubbedClock clock = new();

    public InMemoryBudgetSummariesDataSourceTests()
    {
        this.budgetRepository = this.Resolve<IBudgetRepository, InMemoryBudgetRepository>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddSingleton<IClock>(this.clock);

    [Theory]
    [RandomData]
    public async Task Gives_budget_beginning_today(decimal amount, DateOnly today)
    {
        BudgetBuilder budget = ABudget(today, amount);
        this.TodayIs(today);
        this.Feed(budget);

        await this.Verify(budget with { TotalAmount = amount });
    }

    [Theory]
    [RandomData]
    public async Task Gives_budget_beginning_this_month(decimal amount)
    {
        BudgetBuilder expected = ABudget(DateOnly.Parse("2024-01-12"), amount);
        this.TodayIs(DateOnly.Parse("2024-01-01"));
        this.Feed(expected);

        await this.Verify(expected with { TotalAmount = amount });
    }

    [Fact]
    public async Task Gives_budgets_beginning_last_month()
    {
        BudgetBuilder budget = ABudget(DateOnly.Parse("2023-12-10"), 12);
        this.TodayIs(DateOnly.Parse("2024-01-01"));
        this.Feed(budget);

        await this.Verify(budget with { TotalAmount = 24 });
    }

    [Theory]
    [RandomData]
    public async Task Gives_budgets_beginning_next_month(decimal amount)
    {
        BudgetBuilder budget = ABudget(DateOnly.Parse("2024-02-01"), amount);
        this.TodayIs(DateOnly.Parse("2024-01-01"));
        this.Feed(budget);

        await this.Verify(budget with { TotalAmount = 0 });
    }

    private async Task Verify(BudgetBuilder expected)
    {
        BudgetSummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(expected.ToSummary());
    }

    private void Feed(BudgetBuilder expected) =>
        this.budgetRepository.Feed(expected.ToSnapshot());

    private void TodayIs(DateOnly today) =>
        this.clock.Today = today;

    private static BudgetBuilder ABudget(DateOnly beginDate, decimal amount) =>
        BuilderHelpers.ABudget() with { BeginDate = beginDate, Amount = amount };
}
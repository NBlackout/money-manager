using Microsoft.Extensions.DependencyInjection;
using Shared.Infra.TestTooling.TestDoubles;
using Shared.Ports;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public class
    InMemoryBudgetSummariesDataSourceTests : InfraTest<IBudgetSummariesDataSource, InMemoryBudgetSummariesDataSource>
{
    private readonly InMemoryBudgetRepository budgetRepository;
    private readonly StubbedDateOnlyProvider dateOnlyProvider = new();

    public InMemoryBudgetSummariesDataSourceTests()
    {
        this.budgetRepository = this.Resolve<IBudgetRepository, InMemoryBudgetRepository>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddSingleton<IDateOnlyProvider>(this.dateOnlyProvider);

    [Theory, RandomData]
    public async Task Gives_budget_beginning_today(decimal amount, DateOnly today)
    {
        BudgetBuilder budget = ABudget(today, amount);
        this.TodayIs(today);
        this.Feed(budget);

        await this.Verify(budget with { TotalAmount = amount });
    }

    [Theory, RandomData]
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

    [Theory, RandomData]
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
        this.dateOnlyProvider.Today = today;

    private static BudgetBuilder ABudget(DateOnly beginDate, decimal amount) =>
        Any<BudgetBuilder>() with { BeginDate = beginDate, Amount = amount };
}
using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.BudgetSummaries;
using Shared.Infra.DateOnlyProvider;
using Shared.Infra.TestTooling;
using Shared.Ports;
using Write.Infra.Repositories;
using static Shared.TestTooling.Randomizer;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryBudgetSummariesDataSourceTests : HostFixture
{
    private static readonly DateOnly Today = DateOnly.Parse("2024-01-01");
    private static readonly DateOnly ThisMonth = DateOnly.Parse("2024-01-12");
    private static readonly DateOnly LastMonth = DateOnly.Parse("2023-12-10");
    private static readonly DateOnly NextMonth = DateOnly.Parse("2024-02-01");

    private readonly RepositoryBudgetSummariesDataSource sut;
    private readonly InMemoryBudgetRepository budgetRepository;
    private readonly StubbedDateOnlyProvider dateOnlyProvider = new(Today);

    public RepositoryBudgetSummariesDataSourceTests()
    {
        this.sut = this.Resolve<IBudgetSummariesDataSource, RepositoryBudgetSummariesDataSource>();
        this.budgetRepository = this.Resolve<IBudgetRepository, InMemoryBudgetRepository>();
    }

    protected override void Configure(IServiceCollection services)
    {
        services.AddSingleton<IDateOnlyProvider>(this.dateOnlyProvider);
    }

    [Theory, RandomData]
    public async Task Retrieves_budget_beginning_today(decimal amount)
    {
        BudgetBuilder expected = ABudget() with { Amount = amount, BeginDate = Today, TotalAmount = amount };
        this.Feed(expected);

        await this.Verify(expected);
    }

    [Theory, RandomData]
    public async Task Retrieves_budget_beginning_this_month(decimal amount)
    {
        BudgetBuilder expected = ABudget() with { Amount = amount, BeginDate = ThisMonth, TotalAmount = amount };
        this.Feed(expected);

        await this.Verify(expected);
    }


    [Fact]
    public async Task Retrieves_budgets_beginning_last_month()
    {
        BudgetBuilder expected = ABudget() with { Amount = 12, BeginDate = LastMonth, TotalAmount = 24 };
        this.Feed(expected);

        await this.Verify(expected);
    }

    [Fact]
    public async Task Retrieves_budgets_beginning_next_month()
    {
        BudgetBuilder expected = ABudget() with { BeginDate = NextMonth, TotalAmount = 0 };
        this.Feed(expected);

        await this.Verify(expected);
    }

    private async Task Verify(BudgetBuilder expected)
    {
        BudgetSummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(expected.ToSummary());
    }

    private void Feed(BudgetBuilder expected) =>
        this.budgetRepository.Feed(expected.ToSnapshot());

    private static BudgetBuilder ABudget() =>
        Any<BudgetBuilder>();
}
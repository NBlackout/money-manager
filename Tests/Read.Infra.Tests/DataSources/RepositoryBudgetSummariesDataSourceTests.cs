using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.BudgetSummaries;
using Write.Infra;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryBudgetSummariesDataSourceTests : HostFixture
{
    private readonly RepositoryBudgetSummariesDataSource sut;
    private readonly InMemoryBudgetRepository budgetRepository;

    public RepositoryBudgetSummariesDataSourceTests()
    {
        this.sut = this.Resolve<IBudgetSummariesDataSource, RepositoryBudgetSummariesDataSource>();
        this.budgetRepository = this.Resolve<IBudgetRepository, InMemoryBudgetRepository>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteInfra().AddReadInfra();

    [Theory, RandomData]
    public async Task Retrieves_budgets(BudgetBuilder[] expected)
    {
        this.budgetRepository.Feed(expected.Select(c => c.ToSnapshot()).ToArray());
        BudgetSummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(expected.Select(c => c.ToSummary()));
    }
}

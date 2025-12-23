using App.Read.Ports;
using App.Shared;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;
using static App.Tests.Read.Tooling.BuilderHelpers;

namespace Infra.Tests.Read.DataSources;

public class InMemorySamplePeriodsDataSourceTests : InfraTest<ISamplePeriodsDataSource, InMemorySamplePeriodsDataSource>
{
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemorySamplePeriodsDataSourceTests()
    {
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Gives_total_amount_per_period()
    {
        Period aPeriod = new(DateOnly.Parse("2028-07-19"), DateOnly.Parse("2028-07-21"));
        Period anotherPeriod = new(DateOnly.Parse("2028-09-04"), DateOnly.Parse("2028-10-11"));
        this.Feed(
            ATransaction() with { Date = DateOnly.Parse("2028-07-19"), Amount = -18 },
            ATransaction() with { Date = DateOnly.Parse("2028-07-20"), Amount = 2 },
            ATransaction() with { Date = DateOnly.Parse("2028-09-29"), Amount = 180 },
            ATransaction() with { Date = DateOnly.Parse("2028-10-11"), Amount = -30 }
        );
        await this.Verify([aPeriod, anotherPeriod], new SamplePeriod(aPeriod, -16), new SamplePeriod(anotherPeriod, 150));
    }

    [Theory, RandomData]
    public async Task Tells_when_there_is_no_activity_during(Period period)
    {
        this.Feed(ATransaction() with { Date = period.From.AddDays(-1) }, ATransaction() with { Date = period.To.AddDays(1) });
        await this.Verify([period], new SamplePeriod(period, 0));
    }

    private async Task Verify(Period[] date, params SamplePeriod[] expected)
    {
        SamplePeriod[] actual = await this.Sut.Of(date);
        actual.Should().Equal(expected);
    }

    private void Feed(params TransactionBuilder[] transactions) =>
        this.transactionRepository.Feed(transactions.Select(t => t.ToSnapshot()).ToArray());
}
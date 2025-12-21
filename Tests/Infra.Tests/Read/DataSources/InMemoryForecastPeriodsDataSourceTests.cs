using App.Read.Ports;
using App.Shared;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;
using static App.Tests.Read.Tooling.BuilderHelpers;

namespace Infra.Tests.Read.DataSources;

public class InMemoryForecastPeriodsDataSourceTests : InfraTest<IForecastPeriodsDataSource, InMemoryForecastPeriodsDataSource>
{
    private readonly InMemoryRecurringTransactionRepository recurringTransactionRepository;

    public InMemoryForecastPeriodsDataSourceTests() =>
        this.recurringTransactionRepository = this.Resolve<IRecurringTransactionRepository, InMemoryRecurringTransactionRepository>();

    [Theory, RandomData]
    public async Task Gives_recurring_transaction(Period aPeriod, Period anotherPeriod)
    {
        this.Feed(ARecurringTransaction() with { Amount = -40 }, ARecurringTransaction() with { Amount = -60 });
        await this.Verify([aPeriod, anotherPeriod], new ForecastPeriod(aPeriod, -100), new ForecastPeriod(anotherPeriod, -100));
    }

    [Theory, RandomData]
    public async Task Tells_when_there_is_no_recurring_transaction(Period period) =>
        await this.Verify([period], new ForecastPeriod(period, 0));

    private async Task Verify(Period[] date, params ForecastPeriod[] expected)
    {
        ForecastPeriod[] actual = await this.Sut.Of(date);
        actual.Should().Equal(expected);
    }

    private void Feed(params RecurringTransactionBuilder[] recurringTransactions) =>
        this.recurringTransactionRepository.Feed(recurringTransactions.Select(t => t.ToSnapshot()).ToArray());
}
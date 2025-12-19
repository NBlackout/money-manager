using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;
using static App.Tests.Read.Tooling.BuilderHelpers;

namespace Infra.Tests.Read.DataSources;

public class InMemoryPerformanceForecastDataSourceTests : InfraTest<IPerformanceForecastDataSource, InMemoryPerformanceForecastDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryRecurringTransactionRepository recurringTransactionRepository;

    public InMemoryPerformanceForecastDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
        this.recurringTransactionRepository = this.Resolve<IRecurringTransactionRepository, InMemoryRecurringTransactionRepository>();
    }

    [Fact]
    public async Task Gives_total_balance()
    {
        AccountBuilder anAccount = AnAccount() with { Balance = 1000 };
        AccountBuilder anotherAccount = AnAccount() with { Balance = 2000 };
        this.Feed([anAccount, anotherAccount]);

        await this.Verify(new PerformanceForecastPresentation(3000, 0, 0, 0));
    }

    [Fact]
    public async Task Gives_forecast_based_on_recurring_transactions()
    {
        RecurringTransactionBuilder aRecurringIncome = ARecurringTransaction() with { Amount = 25 };
        RecurringTransactionBuilder anotherRecurringIncome = ARecurringTransaction() with { Amount = 75 };
        RecurringTransactionBuilder aRecurringExpense = ARecurringTransaction() with { Amount = -300 };
        RecurringTransactionBuilder anotherRecurringExpense = ARecurringTransaction() with { Amount = -100 };
        this.Feed([aRecurringIncome, anotherRecurringIncome, aRecurringExpense, anotherRecurringExpense]);

        await this.Verify(new PerformanceForecastPresentation(0, 100, -400, -300));
    }

    private async Task Verify(PerformanceForecastPresentation expected)
    {
        PerformanceForecastPresentation actual = await this.Sut.Fetch();
        actual.Should().Be(expected);
    }

    private void Feed(AccountBuilder[] accounts) =>
        this.accountRepository.Feed([..accounts.Select(a => a.ToSnapshot())]);

    private void Feed(RecurringTransactionBuilder[] transactions) =>
        this.recurringTransactionRepository.Feed([..transactions.Select(t => t.ToSnapshot())]);
}
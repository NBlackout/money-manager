using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;
using static App.Tests.Read.Tooling.BuilderHelpers;

namespace Infra.Tests.Read.DataSources;

public class InMemoryBalanceForecastsDataSourceTests : InfraTest<IBalanceForecastsDataSource, InMemoryBalanceForecastsDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryRecurringTransactionRepository recurringTransactionRepository;

    public InMemoryBalanceForecastsDataSourceTests()
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

        await this.Verify(
            new BalanceForecastPresentation(3000),
            new BalanceForecastPresentation(3000),
            new BalanceForecastPresentation(3000),
            new BalanceForecastPresentation(3000)
        );
    }

    [Fact]
    public async Task Gives_forecast_based_on_recurring_transactions()
    {
        RecurringTransactionBuilder aRecurringIncome = ARecurringTransaction() with { Amount = 25 };
        RecurringTransactionBuilder anotherRecurringIncome = ARecurringTransaction() with { Amount = 75 };
        RecurringTransactionBuilder aRecurringExpense = ARecurringTransaction() with { Amount = -300 };
        RecurringTransactionBuilder anotherRecurringExpense = ARecurringTransaction() with { Amount = -100 };
        this.Feed([aRecurringIncome, anotherRecurringIncome, aRecurringExpense, anotherRecurringExpense]);

        await this.Verify(
            new BalanceForecastPresentation(-300),
            new BalanceForecastPresentation(-600),
            new BalanceForecastPresentation(-900),
            new BalanceForecastPresentation(-1200)
        );
    }

    private async Task Verify(params BalanceForecastPresentation[] expected)
    {
        BalanceForecastPresentation[] actual = await this.Sut.Fetch();
        actual.Should().Equal(expected);
    }

    private void Feed(AccountBuilder[] accounts) =>
        this.accountRepository.Feed([..accounts.Select(a => a.ToSnapshot())]);

    private void Feed(RecurringTransactionBuilder[] transactions) =>
        this.recurringTransactionRepository.Feed([..transactions.Select(t => t.ToSnapshot())]);
}
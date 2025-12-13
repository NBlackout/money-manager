using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryPerformanceForecastDataSourceTests : InfraTest<IPerformanceForecastDataSource, InMemoryPerformanceForecastDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemoryPerformanceForecastDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
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
        TransactionBuilder aRecurringIncome = ARecurringTransaction() with { Amount = 25 };
        TransactionBuilder anotherRecurringIncome = ARecurringTransaction() with { Amount = 75 };
        TransactionBuilder aRecurringExpense = ARecurringTransaction() with { Amount = -300 };
        TransactionBuilder anotherRecurringExpense = ARecurringTransaction() with { Amount = -100 };
        TransactionBuilder notRecurringTransaction = ATransaction() with { IsRecurring = false };
        this.Feed([aRecurringIncome, anotherRecurringIncome, aRecurringExpense, anotherRecurringExpense, notRecurringTransaction]);

        await this.Verify(new PerformanceForecastPresentation(0, 100, -400, -300));
    }

    private async Task Verify(PerformanceForecastPresentation expected)
    {
        PerformanceForecastPresentation actual = await this.Sut.Fetch();
        actual.Should().Be(expected);
    }

    private void Feed(AccountBuilder[] accounts) =>
        this.accountRepository.Feed([..accounts.Select(a => a.ToSnapshot())]);

    private void Feed(TransactionBuilder[] transactions) =>
        this.transactionRepository.Feed([..transactions.Select(t => t.ToSnapshot())]);

    private static AccountBuilder AnAccount() =>
        Any<AccountBuilder>();

    private static TransactionBuilder ATransaction() =>
        Any<TransactionBuilder>();

    private static TransactionBuilder ARecurringTransaction() =>
        ATransaction() with { IsRecurring = true };
}
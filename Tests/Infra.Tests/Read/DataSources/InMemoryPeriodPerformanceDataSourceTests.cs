using App.Read.Ports;
using App.Shared;
using App.Write.Model.Accounts;
using App.Write.Model.Transactions;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryPeriodPerformanceDataSourceTests : InfraTest<IPeriodPerformanceDataSource, InMemoryPeriodPerformanceDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemoryPeriodPerformanceDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Gives_balance_having_no_transaction()
    {
        AccountSnapshot account = AnAccount() with { Balance = 1200, BalanceDate = DateOnly.Parse("2025-09-28") };
        this.Feed(account);

        await this.Verify(
            [new DateRange(DateOnly.Parse("2024-09-01"), DateOnly.Parse("2024-09-01"))],
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2024-09-01"), DateOnly.Parse("2024-09-01")),
                account.Balance,
                new PerformancePresentation(0, 0, 0)
            )
        );
    }

    [Fact]
    public async Task Gives_balance_at_the_start_of_month()
    {
        AccountSnapshot account = AnAccount() with { Balance = 1200, BalanceDate = DateOnly.Parse("2025-04-12") };
        this.Feed(
            account,
            ATransactionOf(account) with { Amount = 200, Date = DateOnly.Parse("2024-04-10") },
            ATransactionOf(account) with { Amount = 300, Date = DateOnly.Parse("2024-04-03") }
        );

        await this.Verify(
            [new DateRange(DateOnly.Parse("2024-04-01"), DateOnly.Parse("2024-04-30"))],
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2024-04-01"), DateOnly.Parse("2024-04-30")),
                700,
                new PerformancePresentation(500, 0, 500)
            )
        );
    }

    [Fact]
    public async Task Gives_balance_starting_from_last_month()
    {
        AccountSnapshot account = AnAccount() with { Balance = 500, BalanceDate = DateOnly.Parse("2022-01-14") };
        this.Feed(
            account,
            ATransactionOf(account) with { Amount = 80, Date = DateOnly.Parse("2022-01-19") },
            ATransactionOf(account) with { Amount = 15, Date = DateOnly.Parse("2021-12-18") }
        );

        await this.Verify(
            [
                new DateRange(DateOnly.Parse("2021-12-01"), DateOnly.Parse("2021-12-31")),
                new DateRange(DateOnly.Parse("2022-01-01"), DateOnly.Parse("2022-01-31"))
            ],
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2021-12-01"), DateOnly.Parse("2021-12-31")),
                405,
                new PerformancePresentation(15, 0, 15)
            ),
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2022-01-01"), DateOnly.Parse("2022-01-31")),
                420,
                new PerformancePresentation(80, 0, 80)
            )
        );
    }

    [Fact]
    public async Task Gives_balance_without_transaction_on_period()
    {
        AccountSnapshot account = AnAccount() with { Balance = 130, BalanceDate = DateOnly.Parse("2024-08-17") };
        this.Feed(account, ATransactionOf(account) with { Amount = 80, Date = DateOnly.Parse("2024-06-30") });

        await this.Verify(
            [
                new DateRange(DateOnly.Parse("2024-06-01"), DateOnly.Parse("2024-06-30")),
                new DateRange(DateOnly.Parse("2024-07-01"), DateOnly.Parse("2024-07-31")),
                new DateRange(DateOnly.Parse("2024-08-01"), DateOnly.Parse("2024-08-31"))
            ],
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2024-06-01"), DateOnly.Parse("2024-06-30")),
                50,
                new PerformancePresentation(80, 0, 80)
            ),
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2024-07-01"), DateOnly.Parse("2024-07-31")),
                130,
                new PerformancePresentation(0, 0, 0)
            ),
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2024-08-01"), DateOnly.Parse("2024-08-31")),
                130,
                new PerformancePresentation(0, 0, 0)
            )
        );
    }

    [Fact]
    public async Task Gives_multiple_balances()
    {
        AccountSnapshot anAccount = AnAccount() with { BalanceDate = DateOnly.Parse("2025-09-28") };
        AccountSnapshot anotherAccount = AnAccount() with { BalanceDate = DateOnly.Parse("2025-09-28") };
        this.Feed([anAccount, anotherAccount]);

        await this.Verify(
            [new DateRange(DateOnly.Parse("2024-09-01"), DateOnly.Parse("2024-09-30"))],
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2024-09-01"), DateOnly.Parse("2024-09-30")),
                anAccount.Balance + anotherAccount.Balance,
                new PerformancePresentation(0, 0, 0)
            )
        );
    }

    [Fact]
    public async Task Gives_multiple_balances_across_many_months()
    {
        AccountSnapshot anAccount = AnAccount() with { Balance = 3000, BalanceDate = DateOnly.Parse("2017-03-13") };
        AccountSnapshot anotherAccount = AnAccount() with { Balance = 1500, BalanceDate = DateOnly.Parse("2017-02-04") };
        this.Feed(
            [anAccount, anotherAccount],
            ATransactionOf(anAccount) with { Amount = 500, Date = DateOnly.Parse("2017-03-11") },
            ATransactionOf(anAccount) with { Amount = 200, Date = DateOnly.Parse("2017-02-19") },
            ATransactionOf(anotherAccount) with { Amount = 800, Date = DateOnly.Parse("2017-02-09") },
            ATransactionOf(anotherAccount) with { Amount = 1000, Date = DateOnly.Parse("2017-01-02") }
        );

        await this.Verify(
            [
                new DateRange(DateOnly.Parse("2017-01-01"), DateOnly.Parse("2017-01-31")),
                new DateRange(DateOnly.Parse("2017-02-01"), DateOnly.Parse("2017-02-28")),
                new DateRange(DateOnly.Parse("2017-03-01"), DateOnly.Parse("2017-03-31"))
            ],
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2017-01-01"), DateOnly.Parse("2017-01-31")),
                2000,
                new PerformancePresentation(1000, 0, 1000)
            ),
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2017-02-01"), DateOnly.Parse("2017-02-28")),
                3000,
                new PerformancePresentation(1000, 0, 1000)
            ),
            new PeriodPerformancePresentation(
                new DateRange(DateOnly.Parse("2017-03-01"), DateOnly.Parse("2017-03-31")),
                4000,
                new PerformancePresentation(500, 0, 500)
            )
        );
    }

    private async Task Verify(DateRange[] dateRanges, params PeriodPerformancePresentation[] expected)
    {
        PeriodPerformancePresentation[] actual = await this.Sut.All(dateRanges);
        actual.Should().Equal(expected);
    }

    private void Feed(AccountSnapshot account, params TransactionSnapshot[] transactions) =>
        this.Feed([account], transactions);

    private void Feed(AccountSnapshot[] accounts, params TransactionSnapshot[] transactions)
    {
        this.accountRepository.Feed(accounts);
        this.transactionRepository.Feed(transactions);
    }

    private static AccountSnapshot AnAccount() =>
        Any<AccountSnapshot>();

    private static TransactionSnapshot ATransactionOf(AccountSnapshot account) =>
        Any<TransactionSnapshot>() with { AccountId = account.Id };
}
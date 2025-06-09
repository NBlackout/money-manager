using Write.App.Model.Accounts;
using Write.App.Model.Transactions;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

// Many account
// Many transaction of a multiple months
// Balance date august, only transaction in june. What happens for july?
// Minimum date
// No coherence between baseline and startingFrom (we can have for example baseLine middle of month and startingFrom end of month) -> we might want a VO
public class InMemorySlidingBalancesDataSourceTests
    : InfraTest<ISlidingBalancesDataSource, InMemorySlidingBalancesDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemorySlidingBalancesDataSourceTests()
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
            DateOnly.Parse("2024-09-01"),
            DateOnly.Parse("2024-09-01"),
            PresentationFrom(DateOnly.Parse("2024-09-01"), account)
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
            DateOnly.Parse("2024-04-01"),
            DateOnly.Parse("2024-04-01"),
            PresentationFrom(DateOnly.Parse("2024-04-01"), account with { Balance = 700 })
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
            DateOnly.Parse("2022-01-01"),
            DateOnly.Parse("2021-12-01"),
            PresentationFrom(DateOnly.Parse("2021-12-01"), account with { Balance = 405 }),
            PresentationFrom(DateOnly.Parse("2022-01-01"), account with { Balance = 420 })
        );
    }

    [Fact]
    public async Task Gives_balance_without_transaction_on_period()
    {
        AccountSnapshot account = AnAccount() with { Balance = 130, BalanceDate = DateOnly.Parse("2024-08-17") };
        this.Feed(account, ATransactionOf(account) with { Amount = 80, Date = DateOnly.Parse("2024-06-30") });

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-06-01"),
            PresentationFrom(DateOnly.Parse("2024-06-01"), account with { Balance = 50 }),
            PresentationFrom(DateOnly.Parse("2024-07-01"), account with { Balance = 130 }),
            PresentationFrom(DateOnly.Parse("2024-08-01"), account with { Balance = 130 })
        );
    }

    [Fact]
    public async Task Gives_multiple_balances()
    {
        AccountSnapshot anAccount = AnAccount() with { BalanceDate = DateOnly.Parse("2025-09-28") };
        AccountSnapshot anotherAccount = AnAccount() with { BalanceDate = DateOnly.Parse("2025-09-28") };
        this.Feed([anAccount, anotherAccount]);

        await this.Verify(
            DateOnly.Parse("2024-09-01"),
            DateOnly.Parse("2024-09-01"),
            PresentationFrom(DateOnly.Parse("2024-09-01"), anAccount, anotherAccount)
        );
    }

    [Fact]
    public async Task Gives_multiple_balances_across_many_months()
    {
        AccountSnapshot anAccount = AnAccount() with { Balance = 3000, BalanceDate = DateOnly.Parse("2017-03-13") };
        AccountSnapshot anotherAccount =
            AnAccount() with { Balance = 1500, BalanceDate = DateOnly.Parse("2017-02-04") };
        this.Feed(
            [anAccount, anotherAccount],
            ATransactionOf(anAccount) with { Amount = 500, Date = DateOnly.Parse("2017-03-11") },
            ATransactionOf(anAccount) with { Amount = 200, Date = DateOnly.Parse("2017-02-19") },
            ATransactionOf(anotherAccount) with { Amount = 800, Date = DateOnly.Parse("2017-02-09") },
            ATransactionOf(anotherAccount) with { Amount = 1000, Date = DateOnly.Parse("2017-01-02") }
        );

        await this.Verify(
            DateOnly.Parse("2017-03-01"),
            DateOnly.Parse("2017-01-01"),
            PresentationFrom(
                DateOnly.Parse("2017-01-01"),
                anAccount with { Balance = 2300 },
                anotherAccount with { Balance = -300 }
            ),
            PresentationFrom(
                DateOnly.Parse("2017-02-01"),
                anAccount with { Balance = 2300 },
                anotherAccount with { Balance = 700 }
            ),
            PresentationFrom(
                DateOnly.Parse("2017-03-01"),
                anAccount with { Balance = 2500 },
                anotherAccount with { Balance = 1500 }
            )
        );
    }

    private async Task Verify(
        DateOnly baseline,
        DateOnly startingFrom,
        params SlidingBalancePresentation[] slidingBalances)
    {
        SlidingBalancesPresentation actual = await this.Sut.All(baseline, startingFrom);
        actual.Should().BeEquivalentTo(new SlidingBalancesPresentation(slidingBalances));
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

    private static SlidingBalancePresentation PresentationFrom(
        DateOnly balanceDate,
        params AccountSnapshot[] accounts) =>
        new(balanceDate, accounts.Select(PresentationFrom).ToArray());

    private static AccountBalancePresentation PresentationFrom(AccountSnapshot account) =>
        new(account.Label, account.Balance);
}
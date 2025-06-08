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
    public async Task Gives_no_balances_by_default() =>
        await this.Verify(Any<DateOnly>(), Any<DateOnly>(), new SlidingBalancesPresentation());

    [Fact]
    public async Task Gives_account_balance_0()
    {
        AccountSnapshot account = AnAccount() with { BalanceDate = DateOnly.Parse("2024-08-17") };
        this.Feed(account);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-08-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_01()
    {
        AccountSnapshot account = AnAccount() with { BalanceDate = DateOnly.Parse("2024-08-17") };
        this.Feed(account);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-07-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance()
    {
        AccountSnapshot account = AnAccount() with { BalanceDate = DateOnly.Parse("2024-08-17") };
        this.Feed(account);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_1()
    {
        AccountSnapshot account = AnAccount() with
        {
            BalanceAmount = 12000, BalanceDate = DateOnly.Parse("2024-08-17")
        };
        TransactionSnapshot transaction = ATransaction() with { Amount = 2000, Date = DateOnly.Parse("2024-08-10") };
        this.Feed(account, transaction);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, 10000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, 10000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, 10000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, 10000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, 10000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 10000)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_2()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transaction = ATransaction() with { Amount = 200, Date = DateOnly.Parse("2024-08-12") };
        TransactionSnapshot transaction2 = ATransaction() with { Amount = 300, Date = DateOnly.Parse("2024-08-03") };
        this.Feed(account, transaction, transaction2);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_4()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transactionLastYear =
            ATransaction() with { Amount = 300, Date = DateOnly.Parse("2023-08-03") };
        this.Feed(account, transactionLastYear);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_5()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transactionJuly = ATransaction() with { Amount = 300, Date = DateOnly.Parse("2024-07-03") };
        this.Feed(account, transactionJuly);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_6()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transactionAugust =
            ATransaction() with { Amount = 100, Date = DateOnly.Parse("2024-08-13") };
        TransactionSnapshot transactionJuly1 =
            ATransaction() with { Amount = 130, Date = DateOnly.Parse("2024-07-03") };
        TransactionSnapshot transactionJuly2 = ATransaction() with { Amount = 70, Date = DateOnly.Parse("2024-07-02") };
        this.Feed(account, transactionJuly1, transactionJuly2, transactionAugust);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1400)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_7()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transactionAugust =
            ATransaction() with { Amount = 100, Date = DateOnly.Parse("2024-08-13") };
        TransactionSnapshot transactionJuly1 =
            ATransaction() with { Amount = 130, Date = DateOnly.Parse("2024-07-03") };
        TransactionSnapshot transactionJuly2 = ATransaction() with { Amount = 70, Date = DateOnly.Parse("2024-07-02") };
        TransactionSnapshot transactionJune = ATransaction() with { Amount = 50, Date = DateOnly.Parse("2024-06-29") };
        this.Feed(account, transactionJuly1, transactionJuly2, transactionAugust, transactionJune);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, 1150)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, 1150)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, 1150)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, 1150)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, 1200)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1400)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_9()
    {
        AccountSnapshot account = AnAccount() with { BalanceDate = DateOnly.Parse("2024-08-08") };
        TransactionSnapshot transaction = ATransaction() with { Date = DateOnly.Parse("2023-08-10") };
        this.Feed(account, transaction);

        await this.Verify(
            DateOnly.Parse("2024-08-01"),
            DateOnly.Parse("2024-03-01"),
            new SlidingBalancesPresentation(
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-03-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-04-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-05-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-06-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-07-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                ),
                new SlidingBalancePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                )
            )
        );
    }

    private void Feed(AccountSnapshot account, params TransactionSnapshot[] transactions)
    {
        this.accountRepository.Feed(account);
        this.transactionRepository.Feed(transactions);
    }

    private async Task Verify(DateOnly baseline, DateOnly startingFrom, SlidingBalancesPresentation expected)
    {
        SlidingBalancesPresentation actual = await this.Sut.All(baseline, startingFrom);
        actual.Should().BeEquivalentTo(expected);
    }

    private static AccountSnapshot AnAccount() =>
        Any<AccountSnapshot>();

    private static TransactionSnapshot ATransaction() =>
        Any<TransactionSnapshot>();
}
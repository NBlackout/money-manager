using Write.App.Model.Accounts;
using Write.App.Model.Transactions;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public class InMemorySlidingAccountBalancesDataSourceTests
    : InfraTest<ISlidingAccountBalancesDataSource, InMemorySlidingAccountBalancesDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemorySlidingAccountBalancesDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Gives_no_balances_by_default() =>
        await this.Verify(new SlidingAccountBalancesPresentation());

    [Fact]
    public async Task Gives_account_balance()
    {
        AccountSnapshot account = AnAccount() with { BalanceDate = DateOnly.Parse("2024-08-17") };
        this.Feed(account);

        await this.Verify(
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
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
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
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
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1000)
                )
            )
        );
    }

    // Many account
    // Many transaction of a single month (beginning of month)
    // Many transaction of a multiple months

    [Fact]
    public async Task Gives_account_balance_3()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transactionJuly = ATransaction() with { Amount = 300, Date = DateOnly.Parse("2024-07-03") };
        this.Feed(account, transactionJuly);

        await this.Verify(
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                )
            )
        );
    }

    [Fact]
    public async Task Gives_account_balance_4()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 1500, BalanceDate = DateOnly.Parse("2024-08-17") };
        TransactionSnapshot transactionLastYear = ATransaction() with { Amount = 300, Date = DateOnly.Parse("2023-08-03") };
        this.Feed(account, transactionLastYear);

        await this.Verify(
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
                    DateOnly.Parse("2024-08-01"),
                    new AccountBalancePresentation(account.Label, 1500)
                )
            )
        );
    }

    private void Feed(AccountSnapshot account, params TransactionSnapshot[] transactions)
    {
        this.accountRepository.Feed(account);
        this.transactionRepository.Feed(transactions);
    }

    private async Task Verify(SlidingAccountBalancesPresentation expected)
    {
        SlidingAccountBalancesPresentation actual = await this.Sut.All();
        actual.Should().BeEquivalentTo(expected);
    }

    private static AccountSnapshot AnAccount() =>
        Any<AccountSnapshot>();

    private static TransactionSnapshot ATransaction() =>
        Any<TransactionSnapshot>();
}
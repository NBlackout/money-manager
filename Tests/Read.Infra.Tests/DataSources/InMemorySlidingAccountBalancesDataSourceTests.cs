using Write.App.Model.Accounts;
using Write.App.Model.Transactions;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public class InMemorySlidingAccountBalancesDataSourceTests :
    InfraTest<ISlidingAccountBalancesDataSource, InMemorySlidingAccountBalancesDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemorySlidingAccountBalancesDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Gives_no_balances_by_default()
    {
        await this.Verify(new SlidingAccountBalancesPresentation([]));
    }

    [Fact]
    public async Task Gives_account_balance()
    {
        AccountSnapshot account = AnAccount() with { BalanceAmount = 12000 };
        TransactionSnapshot transaction = ATransaction() with { Amount = 2000 };
        this.Feed(account, transaction);

        await this.Verify(
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
                    account.BalanceDate,
                    new AccountBalancePresentation(account.Label, 10000)
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
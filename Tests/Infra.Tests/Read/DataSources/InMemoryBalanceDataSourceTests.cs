using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;
using static App.Tests.Read.Tooling.BuilderHelpers;

namespace Infra.Tests.Read.DataSources;

public class InMemoryBalanceDataSourceTests : InfraTest<IBalanceDataSource, InMemoryBalanceDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemoryBalanceDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Gives_balance_on_given_date()
    {
        this.Feed(AnAccount() with { Balance = 1000 });
        this.Feed(
            ATransaction() with { Date = DateOnly.Parse("2028-09-10"), Amount = 20 },
            ATransaction() with { Date = DateOnly.Parse("2028-09-04"), Amount = 180 }
        );
        await this.Verify(DateOnly.Parse("2028-09-04"), 800);
    }

    [Fact]
    public async Task Excludes_too_old_transactions()
    {
        this.Feed(AnAccount() with { Balance = 1000 });
        this.Feed(ATransaction() with { Date = DateOnly.Parse("2011-12-16") });
        await this.Verify(DateOnly.Parse("2021-12-17"), 1000);
    }

    [Theory, RandomData]
    public async Task Tells_when_there_is_no_activity_since(DateOnly date)
    {
        this.Feed(AnAccount() with { Balance = 850 }, AnAccount() with { Balance = 100 });
        await this.Verify(date, 950);
    }

    private async Task Verify(DateOnly date, decimal expected)
    {
        decimal actual = await this.Sut.On(date);
        actual.Should().Be(expected);
    }

    private void Feed(params AccountBuilder[] accounts) =>
        this.accountRepository.Feed(accounts.Select(a => a.ToSnapshot()).ToArray());

    private void Feed(params TransactionBuilder[] transactions) =>
        this.transactionRepository.Feed(transactions.Select(t => t.ToSnapshot()).ToArray());
}
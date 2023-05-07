using MoneyManager.Read.Infrastructure.DataSources.AccountDetails;
using MoneyManager.Write.Api.Extensions;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests.DataSources;

public sealed class RepositoryAccountDetailsDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryAccountDetailsDataSource sut;
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public RepositoryAccountDetailsDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.Service<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>();
        this.accountRepository = this.host.Service<IAccountRepository, InMemoryAccountRepository>();
        this.transactionRepository = this.host.Service<ITransactionRepository, InMemoryTransactionRepository>();

        this.accountRepository.Clear();
        this.transactionRepository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with { Label = "My account", Balance = 142.52m };
        TransactionBuilder transaction = TransactionBuilder.For(Guid.NewGuid()) with { AccountId = account.Id };
        TransactionBuilder otherTransaction = TransactionBuilder.For(Guid.NewGuid()) with { AccountId = account.Id };
        TransactionBuilder otherAccountTransaction =
            TransactionBuilder.For(Guid.NewGuid()) with { AccountId = Guid.NewGuid() };

        this.accountRepository.Feed(account.Build());
        this.transactionRepository.Feed(transaction.Build(), otherTransaction.Build(), otherAccountTransaction.Build());

        AccountDetailsPresentation actual = await this.sut.Get(account.Id);
        actual.Should().BeEquivalentTo(PresentationFrom(account, transaction, otherTransaction));
    }

    private static AccountDetailsPresentation PresentationFrom(AccountBuilder account,
        params TransactionBuilder[] transactions)
    {
        return new AccountDetailsPresentation(account.Id, account.Label, account.Balance,
            transactions.Select(t => new TransactionSummary(t.Id, t.Amount, t.Label)).ToArray());
    }

    public void Dispose() =>
        this.host.Dispose();
}
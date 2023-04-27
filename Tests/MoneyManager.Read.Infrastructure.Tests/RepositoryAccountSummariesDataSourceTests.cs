using MoneyManager.Write.Application.Ports;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests;

public sealed class RepositoryAccountSummariesDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryAccountSummariesDataSource sut;
    private readonly InMemoryAccountRepository repository;

    public RepositoryAccountSummariesDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.GetRequiredService<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
        this.repository = this.host.GetRequiredService<IAccountRepository, InMemoryAccountRepository>();
        this.repository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_tracked_account_summaries()
    {
        AccountBuilder anAccount = AccountBuilder.For(Guid.NewGuid()) with
        {
            Label = "A label", Balance = 10.44m, Tracked = true
        };
        AccountBuilder anotherAccount = AccountBuilder.For(Guid.NewGuid()) with
        {
            Label = "A label", Balance = 656.98m, Tracked = true
        };
        AccountBuilder notTrackedAccount = AccountBuilder.For(Guid.NewGuid()) with
        {
            Label = "This one is not tracked", Balance = 1301.51m, Tracked = false
        };
        this.repository.Feed(anAccount.Build(), anotherAccount.Build(), notTrackedAccount.Build());

        IReadOnlyCollection<AccountSummary> actual = await this.sut.Get();
        actual.Should().Equal(
            new AccountSummary(anAccount.Id, anAccount.Label, anAccount.Balance, anAccount.Tracked),
            new AccountSummary(anotherAccount.Id, anotherAccount.Label, anotherAccount.Balance, anotherAccount.Tracked),
            new AccountSummary(notTrackedAccount.Id, notTrackedAccount.Label, notTrackedAccount.Balance,
                notTrackedAccount.Tracked)
        );
    }

    public void Dispose() =>
        this.host.Dispose();
}
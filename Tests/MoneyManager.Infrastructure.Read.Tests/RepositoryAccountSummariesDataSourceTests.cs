using Microsoft.Extensions.Hosting;
using MoneyManager.Api.Extensions;
using MoneyManager.Application.Read.Ports;
using MoneyManager.Application.Write.Model;
using MoneyManager.Application.Write.Ports;
using MoneyManager.Infrastructure.Read.DataSources.AccountSummaries;
using MoneyManager.Infrastructure.Write.Repositories;
using MoneyManager.Shared;

namespace MoneyManager.Infrastructure.Read.Tests;

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
        const string anAccountNumber = "Number";
        const decimal aBalance = 12.34m;
        const bool tracked = true;
        const bool notTracked = false;
        AccountSnapshot anAccount = new(Guid.NewGuid(), "Big bank", anAccountNumber, aBalance, tracked);

        const string anotherAccountNumber = "Unique number";
        const decimal anotherBalance = 56.78m;
        AccountSnapshot anotherAccount = new(Guid.NewGuid(), "Other bank", anotherAccountNumber, anotherBalance, tracked);

        const string notTrackedAccountNumber = "Not tracked";
        const decimal notTrackedBalance = 103.44m;
        AccountSnapshot notTrackedAccount = new(Guid.NewGuid(), "Not tracked", notTrackedAccountNumber, notTrackedBalance, notTracked);
        this.repository.Feed(anAccount, anotherAccount, notTrackedAccount);

        IReadOnlyCollection<AccountSummary> actual = await this.sut.Get();
        actual.Should().Equal(
            new AccountSummary(anAccount.Id, anAccountNumber, aBalance, tracked),
            new AccountSummary(anotherAccount.Id, anotherAccountNumber, anotherBalance, tracked),
            new AccountSummary(notTrackedAccount.Id, notTrackedAccountNumber, notTrackedBalance, notTracked)
        );
    }

    public void Dispose() =>
        this.host.Dispose();
}
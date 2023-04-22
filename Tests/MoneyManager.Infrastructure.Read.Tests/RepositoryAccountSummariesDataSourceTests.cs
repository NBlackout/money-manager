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
    }

    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        const string anAccountNumber = "Number";
        const decimal aBalance = 12.34m;
        Account anAccount = new(Guid.NewGuid(), "Big bank", anAccountNumber, aBalance);

        const string anotherAccountNumber = "Unique number";
        const decimal anotherBalance = 56.78m;
        Account anotherAccount = new(Guid.NewGuid(), "National bank", anotherAccountNumber, anotherBalance);
        this.repository.Feed(anAccount, anotherAccount);

        IReadOnlyCollection<AccountSummary> actual = await this.sut.Get();
        actual.Should().Equal(
            new AccountSummary(anAccount.Id, anAccountNumber, aBalance),
            new AccountSummary(anotherAccount.Id, anotherAccountNumber, anotherBalance)
        );
    }

    public void Dispose() =>
        this.host.Dispose();
}
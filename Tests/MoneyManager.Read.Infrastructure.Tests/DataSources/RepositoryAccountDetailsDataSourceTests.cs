using MoneyManager.Read.Infrastructure.DataSources;
using MoneyManager.Shared.Presentation;
using MoneyManager.Write.Application.Ports;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests.DataSources;

public sealed class RepositoryAccountDetailsDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryAccountDetailsDataSource sut;
    private readonly InMemoryAccountRepository repository;

    public RepositoryAccountDetailsDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.GetRequiredService<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>();
        this.repository = this.host.GetRequiredService<IAccountRepository, InMemoryAccountRepository>();

        this.repository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            Label = "My account", Balance = 142.52m
        };
        this.repository.Feed(account.Build());

        AccountDetailsPresentation actual = await this.sut.Get(account.Id);
        actual.Should().Be(new AccountDetailsPresentation(account.Id, account.Label, account.Balance));
    }

    public void Dispose() =>
        this.host.Dispose();
}
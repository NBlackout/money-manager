using MoneyManager.Read.Infrastructure.DataSources.AccountDetails;
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
        this.sut = this.host.Service<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>();
        this.repository = this.host.Service<IAccountRepository, InMemoryAccountRepository>();

        this.repository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid());
        this.repository.Feed(account.Build());

        AccountDetailsPresentation actual = await this.sut.Get(account.Id);
        actual.Should().Be(account.ToDetails());
    }

    public void Dispose() =>
        this.host.Dispose();
}
using Read.App.UseCases;
using Read.Infra.DataSources.AccountDetails;
using Shared.Presentation;

namespace Read.App.Tests.UseCases;

public class AccountDetailsTests
{
    private readonly StubbedAccountDetailsDataSource dataSource = new();
    private readonly AccountDetails sut;

    public AccountDetailsTests()
    {
        this.sut = new AccountDetails(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Retrieves_account_details(AccountDetailsPresentation expected)
    {
        this.dataSource.Feed(expected.Id, expected);
        AccountDetailsPresentation actual = await this.sut.Execute(expected.Id);
        actual.Should().Be(expected);
    }
}
using Read.App.UseCases;
using Read.Infra.DataSources.AccountDetails;
using Read.TestTooling;
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

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountDetailsPresentation expected = AccountBuilder.For(Guid.NewGuid()).ToDetails();
        this.dataSource.Feed(expected.Id, expected);

        AccountDetailsPresentation actual = await this.sut.Execute(expected.Id);
        actual.Should().Be(expected);
    }
}
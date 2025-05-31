namespace Client.Read.App.Tests.UseCases;

public class SlidingAccountBalancesTests
{
    private readonly StubbedDashboardGateway gateway = new();
    private readonly SlidingAccountBalances sut;

    public SlidingAccountBalancesTests()
    {
        this.sut = new SlidingAccountBalances(this.gateway);
    }

    [Theory]
    [RandomData]
    public async Task Gives_sliding_account_balances(SlidingAccountBalancesPresentation expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(SlidingAccountBalancesPresentation expected)
    {
        SlidingAccountBalancesPresentation actual = await this.sut.Execute();
        actual.Should().BeEquivalentTo(expected);
    }

    private void Feed(SlidingAccountBalancesPresentation expected) =>
        this.gateway.Feed(expected);
}
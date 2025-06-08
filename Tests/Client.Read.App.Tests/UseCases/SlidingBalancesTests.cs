namespace Client.Read.App.Tests.UseCases;

public class SlidingBalancesTests
{
    private readonly StubbedDashboardGateway gateway = new();
    private readonly SlidingBalances sut;

    public SlidingBalancesTests()
    {
        this.sut = new SlidingBalances(this.gateway);
    }

    [Theory]
    [RandomData]
    public async Task Gives_sliding_account_balances(SlidingBalancesPresentation expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(SlidingBalancesPresentation expected)
    {
        SlidingBalancesPresentation actual = await this.sut.Execute();
        actual.Should().BeEquivalentTo(expected);
    }

    private void Feed(SlidingBalancesPresentation expected) =>
        this.gateway.Feed(expected);
}
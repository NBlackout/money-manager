namespace Client.Read.App.Tests.TestDoubles;

public class StubbedDashboardGateway : IDashboardGateway
{
    private SlidingBalancesPresentation balances = null!;

    public Task<SlidingBalancesPresentation> SlidingBalances() =>
        Task.FromResult(this.balances);

    public void Feed(SlidingBalancesPresentation expected) =>
        this.balances = expected;
}
namespace Client.Read.App.Tests.TestDoubles;

public class StubbedDashboardGateway : IDashboardGateway
{
    private SlidingAccountBalancesPresentation balances = null!;

    public Task<SlidingAccountBalancesPresentation> SlidingAccountBalances() =>
        Task.FromResult(this.balances);

    public void Feed(SlidingAccountBalancesPresentation expected) =>
        this.balances = expected;
}
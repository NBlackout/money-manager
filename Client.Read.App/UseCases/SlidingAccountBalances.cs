namespace Client.Read.App.UseCases;

public class SlidingAccountBalances(IDashboardGateway gateway)
{
    public async Task<SlidingAccountBalancesPresentation> Execute() =>
        await gateway.SlidingAccountBalances();
}
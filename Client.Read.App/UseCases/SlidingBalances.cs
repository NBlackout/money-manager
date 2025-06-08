namespace Client.Read.App.UseCases;

public class SlidingBalances(IDashboardGateway gateway)
{
    public async Task<SlidingBalancesPresentation> Execute() =>
        await gateway.SlidingBalances();
}
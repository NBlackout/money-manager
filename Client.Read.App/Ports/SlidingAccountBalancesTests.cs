namespace Client.Read.App.Ports;

public interface IDashboardGateway
{
    Task<SlidingBalancesPresentation> SlidingBalances();
}
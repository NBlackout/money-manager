namespace Client.Read.App.Ports;

public interface IDashboardGateway
{
    Task<SlidingAccountBalancesPresentation> SlidingAccountBalances();
}
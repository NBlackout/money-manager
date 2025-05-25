namespace Read.App.Ports;

public interface ISlidingAccountBalancesDataSource
{
    Task<SlidingAccountBalancesPresentation> All();
}
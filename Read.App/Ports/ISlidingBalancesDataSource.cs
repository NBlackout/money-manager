namespace Read.App.Ports;

public interface ISlidingBalancesDataSource
{
    Task<SlidingBalancesPresentation> All(DateOnly baseline, DateOnly startingFrom);
}
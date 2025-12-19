namespace App.Read.Ports;

public interface IBalanceForecastsDataSource
{
    Task<BalanceForecastPresentation[]> Fetch();
}

public record BalanceForecastPresentation(decimal Balance);
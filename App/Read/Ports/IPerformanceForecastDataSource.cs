namespace App.Read.Ports;

public interface IPerformanceForecastDataSource
{
    Task<PerformanceForecastPresentation> Fetch();
}

public record PerformanceForecastPresentation(decimal CurrentBalance, decimal Revenue, decimal Expenses, decimal Net);
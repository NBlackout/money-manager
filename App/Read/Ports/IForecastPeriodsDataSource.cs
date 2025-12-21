using App.Shared;

namespace App.Read.Ports;

public interface IForecastPeriodsDataSource
{
    Task<ForecastPeriod[]> Of(Period[] periods);
}

public record ForecastPeriod(Period Period, decimal Net);
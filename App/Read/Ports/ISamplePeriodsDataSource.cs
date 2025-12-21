using App.Shared;

namespace App.Read.Ports;

public interface ISamplePeriodsDataSource
{
    Task<SamplePeriod[]> Of(Period[] period);
}

public record SamplePeriod(Period Period, decimal Amount);
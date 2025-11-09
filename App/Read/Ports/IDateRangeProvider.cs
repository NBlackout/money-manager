using App.Shared;

namespace App.Read.Ports;

public interface IDateRangeProvider
{
    Task<DateRange[]> RollingTwelveMonths();
}
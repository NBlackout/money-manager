using App.Shared;

namespace App.Read.Ports;

public interface IPeriodProvider
{
    Task<Period[]> RollingTwelveMonths();
    Task<Period[]> LastTwelveMonths();
    Task<Period[]> NextThreeMonths();
}
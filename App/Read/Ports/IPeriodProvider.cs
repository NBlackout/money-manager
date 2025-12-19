using App.Shared;

namespace App.Read.Ports;

public interface IPeriodProvider
{
    Task<Period[]> RollingTwelveMonths();
}
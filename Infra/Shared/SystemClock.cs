using App.Shared.Ports;

namespace Infra.Shared;

public class SystemClock : IClock
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}
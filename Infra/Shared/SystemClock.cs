using App.Shared;
using App.Shared.Ports;

namespace Infra.Shared;

public class SystemClock : IClock
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
    public DateOnly FirstDayOfMonth => this.Today.FirstDayOfMonth();
    public DateOnly LastDayOfMonth => this.Today.LastDayOfMonth();
}
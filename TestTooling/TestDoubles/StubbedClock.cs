using App.Shared.Ports;

namespace TestTooling.TestDoubles;

public class StubbedClock : IClock
{
    public DateOnly Today { get; set; }
    public DateOnly FirstDayOfMonth { get; set; }
    public DateOnly LastDayOfMonth { get; set; }
}
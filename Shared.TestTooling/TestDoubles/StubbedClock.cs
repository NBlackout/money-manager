using Shared.Ports;

namespace Shared.TestTooling.TestDoubles;

public class StubbedClock : IClock
{
    public DateOnly Today { get; set; }
}
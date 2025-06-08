using Shared.Ports;

namespace Shared.Infra.TestTooling.TestDoubles;

public class StubbedClock : IClock
{
    public DateOnly Today { get; set; }
}
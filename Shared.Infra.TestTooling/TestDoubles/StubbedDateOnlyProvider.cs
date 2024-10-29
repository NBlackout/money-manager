using Shared.Ports;

namespace Shared.Infra.TestTooling.TestDoubles;

public class StubbedDateOnlyProvider : IDateOnlyProvider
{
    public DateOnly Today { get; set; }
}
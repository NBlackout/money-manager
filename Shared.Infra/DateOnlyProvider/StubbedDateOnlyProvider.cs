using Shared.Ports;

namespace Shared.Infra.DateOnlyProvider;

public class StubbedDateOnlyProvider(DateOnly today) : IDateOnlyProvider
{
    public DateOnly Today { get; } = today;
}
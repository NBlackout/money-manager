using Shared.Ports;

namespace Shared.Infra.DateOnlyProvider;

public class StubbedDateOnlyProvider : IDateOnlyProvider
{
    public DateOnly Today { get; set; }

    public StubbedDateOnlyProvider()
    {
    }

    public StubbedDateOnlyProvider(DateOnly today) : this()
    {
        this.Today = today;
    }
}
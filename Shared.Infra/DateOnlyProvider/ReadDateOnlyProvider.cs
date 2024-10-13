using Shared.Ports;

namespace Shared.Infra.DateOnlyProvider;

public class ReadDateOnlyProvider : IDateOnlyProvider
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}
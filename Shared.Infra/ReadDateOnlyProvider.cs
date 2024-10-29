namespace Shared.Infra;

public class ReadDateOnlyProvider : IDateOnlyProvider
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}
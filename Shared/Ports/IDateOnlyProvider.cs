namespace Shared.Ports;

public interface IDateOnlyProvider
{
    DateOnly Today { get; }
}
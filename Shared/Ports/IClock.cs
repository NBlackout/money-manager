namespace Shared.Ports;

public interface IClock
{
    DateOnly Today { get; }
}
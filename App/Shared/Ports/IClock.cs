namespace App.Shared.Ports;

public interface IClock
{
    DateOnly Today { get; }
}
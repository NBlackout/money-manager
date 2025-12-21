namespace App.Shared.Ports;

public interface IClock
{
    DateOnly Today { get; }
    DateOnly FirstDayOfMonth { get; }
    DateOnly LastDayOfMonth { get; }
}
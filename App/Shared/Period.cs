namespace App.Shared;

public sealed record Period(DateOnly From, DateOnly To)
{
    public bool Includes(DateOnly date) =>
        date >= this.From && date <= this.To;
}
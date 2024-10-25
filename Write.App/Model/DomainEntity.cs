namespace Write.App.Model;

public abstract class DomainEntity<TId>(TId id)
{
    public TId Id { get; } = id;
}
namespace Write.App.Model;

public abstract class DomainEntity(Guid id)
{
    public Guid Id { get; } = id;
}
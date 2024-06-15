namespace Write.App.Model;

public abstract class DomainEntity
{
    public Guid Id { get; }

    protected DomainEntity(Guid id)
    {
        this.Id = id;
    }
}
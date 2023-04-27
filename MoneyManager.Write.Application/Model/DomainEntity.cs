namespace MoneyManager.Write.Application.Model;

public abstract class DomainEntity
{
    public Guid Id { get; }

    protected DomainEntity(Guid id)
    {
        this.Id = id;
    }
}
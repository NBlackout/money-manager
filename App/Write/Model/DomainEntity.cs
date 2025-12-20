namespace App.Write.Model;

public abstract class DomainEntity<TId, TSnapshot>(TId id) where TSnapshot : ISnapshot<TId>
{
    public TId Id { get; } = id;

    public abstract TSnapshot Snapshot { get; }

    protected DomainEntity(TSnapshot snapshot) : this(snapshot.Id)
    {
    }
}
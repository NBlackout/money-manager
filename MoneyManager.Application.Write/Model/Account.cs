namespace MoneyManager.Application.Write.Model;

public class Account
{
    public Guid Id { get; }
    public ExternalId ExternalId { get; }
    public decimal Balance { get; private set; }

    public Account(Guid id, ExternalId externalId, decimal balance)
    {
        this.Id = id;
        this.ExternalId = externalId;
        this.Balance = balance;
    }

    public void Synchronize(decimal balance)
    {
        this.Balance = balance;
    }
}
namespace MoneyManager.Application.Write.Ports;

public record AccountStatement(ExternalId ExternalId, decimal Balance)
{
    public Account Track(Guid id) =>
        new(id, this.ExternalId, this.Balance);
}
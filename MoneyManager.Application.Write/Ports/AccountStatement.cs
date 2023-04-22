namespace MoneyManager.Application.Write.Ports;

public record AccountStatement(string BankIdentifier, string AccountNumber, decimal Balance)
{
    public Account Track(Guid id) =>
        new(id, this.BankIdentifier, this.AccountNumber, this.Balance);
}
namespace MoneyManager.Application.Write.Ports;

public record AccountStatement(string BankIdentifier, string AccountNumber, decimal Balance)
{
    public Account TrackDescribedAccount(Guid id) =>
        Account.StartTracking(id, this.BankIdentifier, this.AccountNumber, this.Balance);
}
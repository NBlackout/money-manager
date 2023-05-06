namespace MoneyManager.Write.Application.Ports;

public record AccountStatement(string BankIdentifier, string AccountNumber, decimal Balance,
    DateTime BalanceDate)
{
    public Bank TrackDescribedBank(Guid id) =>
        Bank.Track(id, this.BankIdentifier);
}
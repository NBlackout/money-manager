namespace Write.App.Ports;

public record AccountStatement(string BankIdentifier, string AccountNumber, decimal Balance,
    DateTime BalanceDate, params TransactionStatement[] Transactions)
{
    public Bank TrackDescribedBank(Guid id) =>
        new(id, this.BankIdentifier);
}
namespace Write.App.Ports;

public record AccountStatement(
    string AccountNumber,
    decimal Balance,
    DateTime BalanceDate,
    params TransactionStatement[] Transactions);

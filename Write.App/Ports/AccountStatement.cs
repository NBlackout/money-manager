namespace Write.App.Ports;

public record AccountStatement(
    string AccountNumber,
    decimal Balance,
    DateOnly BalanceDate,
    params TransactionStatement[] Transactions);

namespace Write.App.Ports;

public record AccountStatement(string AccountNumber, Balance Balance, params TransactionStatement[] Transactions);
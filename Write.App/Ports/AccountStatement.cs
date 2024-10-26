namespace Write.App.Ports;

public record AccountStatement(ExternalId AccountNumber, Balance Balance, params TransactionStatement[] Transactions);
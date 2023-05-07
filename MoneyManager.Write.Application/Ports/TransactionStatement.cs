namespace MoneyManager.Write.Application.Ports;

public record TransactionStatement(string TransactionIdentifier, decimal Amount, string Label, DateTime Date);
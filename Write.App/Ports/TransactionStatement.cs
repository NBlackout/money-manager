namespace Write.App.Ports;

public record TransactionStatement(string TransactionIdentifier, decimal Amount, string Label, DateTime Date, string? Category);

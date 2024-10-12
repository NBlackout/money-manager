namespace Write.App.Ports;

public record TransactionStatement(string Identifier, decimal Amount, string Label, DateTime Date, string? Category);

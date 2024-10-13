namespace Write.App.Ports;

public record TransactionStatement(string Identifier, decimal Amount, string Label, DateOnly Date, string? Category);

namespace Write.App.Ports;

public record TransactionStatement(string Identifier, decimal Amount, Label Label, DateOnly Date, Label? Category);

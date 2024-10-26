namespace Write.App.Ports;

public record TransactionStatement(ExternalId Identifier, decimal Amount, Label Label, DateOnly Date, Label? Category);

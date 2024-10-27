namespace Write.App.Model.Transactions;

public record TransactionSnapshot(
    TransactionId Id,
    AccountId AccountId,
    string ExternalId,
    decimal Amount,
    string Label,
    DateOnly Date,
    CategoryId? CategoryId
);
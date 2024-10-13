namespace Write.App.Model.Transactions;

public record TransactionSnapshot(Guid Id, Guid AccountId, string ExternalId, decimal Amount, string Label,
    DateOnly Date, Guid? CategoryId);
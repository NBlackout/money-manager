namespace MoneyManager.Write.Application.Model.Transactions;

public record TransactionSnapshot(Guid Id, Guid AccountId, string ExternalId);
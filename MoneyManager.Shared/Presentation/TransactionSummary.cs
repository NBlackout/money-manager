namespace MoneyManager.Shared.Presentation;

public record TransactionSummary(Guid Id, decimal Amount, string Label, DateTime Date);
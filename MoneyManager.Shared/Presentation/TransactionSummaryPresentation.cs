namespace MoneyManager.Shared.Presentation;

public record TransactionSummaryPresentation(Guid Id, decimal Amount, string Label, DateTime Date);
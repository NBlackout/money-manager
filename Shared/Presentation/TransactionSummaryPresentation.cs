namespace Shared.Presentation;

public record TransactionSummaryPresentation(Guid Id, decimal Amount, string Label, DateOnly Date, string? Category);
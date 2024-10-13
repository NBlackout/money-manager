namespace Shared.Presentation;

public record BudgetSummaryPresentation(Guid Id, string Name, decimal Amount, DateOnly BeginDate, decimal TotalAmount);
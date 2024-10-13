namespace Write.App.Model.Budgets;

public record BudgetSnapshot(Guid Id, string Name, decimal Amount, DateOnly BeginDate);
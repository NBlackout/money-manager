namespace App.Write.Model.Budgets;

public record BudgetSnapshot(BudgetId Id, string Name, decimal Amount, DateOnly BeginDate);
using App.Read.Ports;
using App.Write.Model.Budgets;

namespace App.Tests.Read.Tooling;

public record BudgetBuilder(Guid Id, string Name, decimal Amount, DateOnly BeginDate, decimal TotalAmount)
{
    public BudgetSnapshot ToSnapshot() =>
        new(new BudgetId(this.Id), this.Name, this.Amount, this.BeginDate);

    public BudgetSummaryPresentation ToSummary() =>
        new(this.Id, this.Name, this.Amount, this.BeginDate, this.TotalAmount);
}
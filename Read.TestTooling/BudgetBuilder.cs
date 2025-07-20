using System.Diagnostics.CodeAnalysis;
using Read.App.Ports;
using Write.App.Model.Budgets;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record BudgetBuilder(Guid Id, string Name, decimal Amount, DateOnly BeginDate, decimal TotalAmount)
{
    public BudgetSnapshot ToSnapshot() =>
        new(new BudgetId(this.Id), this.Name, this.Amount, this.BeginDate);

    public BudgetSummaryPresentation ToSummary() =>
        new(this.Id, this.Name, this.Amount, this.BeginDate, this.TotalAmount);
}
using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Budgets;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record BudgetBuilder(Guid Id, string Name, decimal Amount, DateOnly BeginDate, decimal TotalAmount)
{
    public BudgetSnapshot ToSnapshot() =>
        new(this.Id, this.Name, this.Amount, this.BeginDate);

    public BudgetSummaryPresentation ToSummary() =>
        new(this.Id, this.Name, this.Amount, this.BeginDate, this.TotalAmount);
}
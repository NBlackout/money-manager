using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Budgets;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record BudgetBuilder(Guid Id, string Name, decimal Amount)
{
    public BudgetSnapshot ToSnapshot() =>
        new(this.Id, this.Name, this.Amount);

    public BudgetSummaryPresentation ToSummary() =>
        new(this.Id, this.Name, this.Amount);
}
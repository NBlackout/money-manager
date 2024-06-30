using Shared.Presentation;
using Write.App.Model.Budgets;
using Write.App.Model.Categories;

namespace Read.TestTooling;

public record BudgetBuilder(Guid Id, string Name, decimal Amount)
{
    public Budget Build() =>
        Budget.From(new BudgetSnapshot(this.Id, this.Name, this.Amount));

    public BudgetSummaryPresentation ToSummary() =>
        new(this.Id, this.Name, this.Amount);
}

using Write.App.Model.Budgets;
using Write.App.Model.Exceptions;
using Write.App.Model.ValueObjects;

namespace Write.Infra.Repositories;

public class InMemoryBudgetRepository : IBudgetRepository
{
    private readonly Dictionary<BudgetId, BudgetSnapshot> data = new();

    public IEnumerable<BudgetSnapshot> Data => this.data.Values.Select(c => c);

    public Budget By(BudgetId id) =>
        Budget.From(this.data[id]);

    public Task EnsureNotAlreadyDefined(Label name)
    {
        if (this.data.Values.Any(b => new Label(b.Name) == name))
            throw new BudgetAlreadyDefinedException();

        return Task.CompletedTask;
    }

    public Task Save(Budget budget)
    {
        this.data[budget.Id] = budget.Snapshot;

        return Task.CompletedTask;
    }

    public void Feed(params BudgetSnapshot[] budgets) =>
        budgets.ToList().ForEach(budget => this.data[budget.Id] = budget);
}
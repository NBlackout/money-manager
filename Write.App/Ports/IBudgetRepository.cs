using Write.App.Model.Budgets;

namespace Write.App.Ports;

public interface IBudgetRepository
{
    Task EnsureNotAlreadyDefined(string name);
    Task Save(Budget budget);
}
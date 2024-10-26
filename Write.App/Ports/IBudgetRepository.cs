using Write.App.Model.Budgets;

namespace Write.App.Ports;

public interface IBudgetRepository
{
    Task EnsureNotAlreadyDefined(Label name);
    Task Save(Budget budget);
}
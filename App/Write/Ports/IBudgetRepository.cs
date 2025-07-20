using App.Write.Model.Budgets;
using App.Write.Model.ValueObjects;

namespace App.Write.Ports;

public interface IBudgetRepository
{
    Task EnsureNotAlreadyDefined(Label name);
    Task Save(Budget budget);
}
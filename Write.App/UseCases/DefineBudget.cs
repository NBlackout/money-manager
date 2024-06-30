using Write.App.Model.Budgets;

namespace Write.App.UseCases;

public class DefineBudget(IBudgetRepository budgetRepository)
{
    public async Task Execute(Guid id, string name, decimal amount)
    {
        await budgetRepository.EnsureNotAlreadyDefined(name);
        await budgetRepository.Save(new Budget(id, name, amount));
    }
}
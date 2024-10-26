using Write.App.Model.Budgets;

namespace Write.App.UseCases;

public class DefineBudget(IBudgetRepository budgetRepository)
{
    public async Task Execute(BudgetId id, Label name, Amount amount, DateOnly beginDate)
    {
        await budgetRepository.EnsureNotAlreadyDefined(name);
        await budgetRepository.Save(new Budget(id, name, amount, beginDate));
    }
}
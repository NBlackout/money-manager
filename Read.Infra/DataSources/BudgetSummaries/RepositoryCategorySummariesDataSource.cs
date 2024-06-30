using Write.Infra.Repositories;

namespace Read.Infra.DataSources.BudgetSummaries;

public class RepositoryBudgetSummariesDataSource(InMemoryBudgetRepository repository) : IBudgetSummariesDataSource
{
    public Task<BudgetSummaryPresentation[]> All()
    {
        BudgetSummaryPresentation[] presentations =
            repository.Data.Select(c => new BudgetSummaryPresentation(c.Id, c.Name, c.Amount)).ToArray();

        return Task.FromResult(presentations);
    }
}
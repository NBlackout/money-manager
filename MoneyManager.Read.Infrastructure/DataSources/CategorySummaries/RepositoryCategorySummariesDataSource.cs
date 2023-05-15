using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.CategorySummaries;

public class RepositoryCategorySummariesDataSource : ICategorySummariesDataSource
{
    private readonly InMemoryCategoryRepository repository;

    public RepositoryCategorySummariesDataSource(InMemoryCategoryRepository repository)
    {
        this.repository = repository;
    }

    public Task<IReadOnlyCollection<CategorySummaryPresentation>> Get()
    {
        IReadOnlyCollection<CategorySummaryPresentation> presentations =
            this.repository.Data.Select(c => new CategorySummaryPresentation(c.Id, c.Label)).ToList();

        return Task.FromResult(presentations);
    }
}
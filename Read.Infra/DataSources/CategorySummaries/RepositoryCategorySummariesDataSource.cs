using Write.Infra.Repositories;

namespace Read.Infra.DataSources.CategorySummaries;

public class RepositoryCategorySummariesDataSource : ICategorySummariesDataSource
{
    private readonly InMemoryCategoryRepository repository;

    public RepositoryCategorySummariesDataSource(InMemoryCategoryRepository repository)
    {
        this.repository = repository;
    }

    public Task<CategorySummaryPresentation[]> Get()
    {
        CategorySummaryPresentation[] presentations =
            this.repository.Data.Select(c => new CategorySummaryPresentation(c.Id, c.Label, c.Keywords)).ToArray();

        return Task.FromResult(presentations);
    }
}
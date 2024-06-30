using Write.Infra.Repositories;

namespace Read.Infra.DataSources.CategorySummaries;

public class RepositoryCategorySummariesDataSource(InMemoryCategoryRepository repository) : ICategorySummariesDataSource
{
    public Task<CategorySummaryPresentation[]> All()
    {
        CategorySummaryPresentation[] presentations =
            repository.Data.Select(c => new CategorySummaryPresentation(c.Id, c.Label, c.Keywords)).ToArray();

        return Task.FromResult(presentations);
    }
}

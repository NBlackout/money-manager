namespace Read.Infra.DataSources;

public class InMemoryCategorySummariesDataSource(InMemoryCategoryRepository repository) : ICategorySummariesDataSource
{
    public Task<CategorySummaryPresentation[]> All()
    {
        CategorySummaryPresentation[] presentations =
            [..repository.Data.Select(c => new CategorySummaryPresentation(c.Id.Value, c.Label, c.Keywords))];

        return Task.FromResult(presentations);
    }
}
using App.Read.Ports;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryCategorySummariesDataSource(InMemoryCategoryRepository repository) : ICategorySummariesDataSource
{
    public Task<CategorySummaryPresentation[]> All()
    {
        CategorySummaryPresentation[] presentations = [..repository.Data.Select(c => new CategorySummaryPresentation(c.Id.Value, c.Label))];

        return Task.FromResult(presentations);
    }
}
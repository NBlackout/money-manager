namespace Read.Infra.DataSources.CategorySummaries;

public class StubbedCategorySummariesDataSource : ICategorySummariesDataSource
{
    private readonly IReadOnlyCollection<CategorySummaryPresentation> data;

    public StubbedCategorySummariesDataSource(IReadOnlyCollection<CategorySummaryPresentation> expected)
    {
        this.data = expected;
    }

    public Task<IReadOnlyCollection<CategorySummaryPresentation>> Get() =>
        Task.FromResult(this.data);
}
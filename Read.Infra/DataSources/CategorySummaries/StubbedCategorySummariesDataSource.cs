namespace Read.Infra.DataSources.CategorySummaries;

public class StubbedCategorySummariesDataSource : ICategorySummariesDataSource
{
    private CategorySummaryPresentation[] data = null!;

    public Task<CategorySummaryPresentation[]> Get() =>
        Task.FromResult(this.data);

    public void Feed(CategorySummaryPresentation[] summaries) =>
        this.data = summaries;
}
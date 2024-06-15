namespace Read.App.UseCases;

public class CategorySummaries
{
    private readonly ICategorySummariesDataSource dataSource;

    public CategorySummaries(ICategorySummariesDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<CategorySummaryPresentation[]> Execute() =>
        await this.dataSource.Get();
}
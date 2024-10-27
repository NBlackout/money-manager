namespace Read.App.UseCases;

public class CategorySummaries(ICategorySummariesDataSource dataSource)
{
    public async Task<CategorySummaryPresentation[]> Execute() =>
        await dataSource.All();
}
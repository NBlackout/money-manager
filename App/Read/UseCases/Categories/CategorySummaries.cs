using App.Read.Ports;

namespace App.Read.UseCases.Categories;

public class CategorySummaries(ICategorySummariesDataSource dataSource)
{
    public async Task<CategorySummaryPresentation[]> Execute() =>
        await dataSource.All();
}
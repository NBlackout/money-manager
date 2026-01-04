using App.Read.Ports;

namespace App.Read.UseCases.Categories;

public class CategoriesExport(ICategorySummariesDataSource dataSource, ICategoryExporter exporter)
{
    public async Task<Stream> Execute()
    {
        CategorySummaryPresentation[] categories = await dataSource.All();

        return await exporter.Export(categories);
    }
}
using App.Read.Ports;
using Infra.Shared;

namespace Infra.Read;

public class CsvCategoryExporter(ICsvHelper csvHelper) : ICategoryExporter
{
    public async Task<Stream> Export(CategorySummaryPresentation[] categories) =>
        await csvHelper.Write(Headers(), [..categories.SelectMany(Row)]);

    private static string[] Headers() => ["Label", "Parent label"];

    private static string[][] Row(CategorySummaryPresentation category) => [[category.Label, ""], ..category.Children.Select(c => Row(c, category.Label))];

    private static string[] Row(ChildCategorySummaryPresentation category, string parentLabel) => [category.Label, parentLabel];
}
using App.Read.Ports;
using Tooling;

namespace Infra.Read;

public class CsvCategoryExporter : ICategoryExporter
{
    private static readonly string LineSeparator = Environment.NewLine;

    public Task<Stream> Export(CategorySummaryPresentation[] categories)
    {
        Console.WriteLine("la");
        string[] rows = [Headers(), ..categories.SelectMany(Row)];
        string content = string.Join(LineSeparator, rows);

        return Task.FromResult(content.ToUtf8Stream());
    }

    private static string Headers() =>
        "Label;Parent label";

    private static string[] Row(CategorySummaryPresentation category) => [category.Label + ";", ..category.Children.Select(c => Row(c, category.Label))];

    private static string Row(ChildCategorySummaryPresentation category, string parentLabel) =>
        category.Label + ";" + parentLabel;
}
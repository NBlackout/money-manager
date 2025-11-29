using App.Read.Ports;
using Tooling;

namespace Infra.Read;

public class CsvCategoryExporter : ICategoryExporter
{
    private static readonly string LineSeparator = Environment.NewLine;

    public Task<Stream> Export(CategorySummaryPresentation[] categories)
    {
        string[] rows = [Headers(), ..categories.Select(Row)];
        string content = string.Join(LineSeparator, rows);

        return Task.FromResult(content.ToUtf8Stream());
    }

    private static string Headers() =>
        "Label";

    private static string Row(CategorySummaryPresentation category) =>
        category.Label;
}
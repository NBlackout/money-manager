using System.Text;
using App.Read.Ports;

namespace Infra.Read;

public class CsvCategoryExporter : ICategoryExporter
{
    private static readonly string LineSeparator = Environment.NewLine;
    private const string CellSeparator = ",";

    public Task<Stream> Export(CategorySummaryPresentation[] categories)
    {
        string[] rows = [Headers(), ..categories.Select(Row)];
        string content = string.Join(LineSeparator, rows);

        return Task.FromResult(Encode(content));
    }

    private static string Headers() =>
        string.Join(CellSeparator, "Label", "Keywords");

    private static string Row(CategorySummaryPresentation category) =>
        string.Join(CellSeparator, category.Label, category.Keywords);

    private static Stream Encode(string content) =>
        new MemoryStream(Encoding.UTF8.GetBytes(content));
}
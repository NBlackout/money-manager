using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace Infra.Write;

public class CsvCategoryImporter : ICategoryImporter
{
    private const string CellSeparator = ";";

    public async Task<CategoryToImport[]> Parse(Stream content)
    {
        using StreamReader reader = new(content);
        string? header = await reader.ReadLineAsync();
        if (header == null)
            return [];

        List<CategoryToImport> categories = [];
        while (await reader.ReadLineAsync() is { } line)
            categories.Add(Parse(line));

        return [..categories];
    }

    private static CategoryToImport Parse(string line)
    {
        string[] segments = line.Split(CellSeparator);
        Label label = new(segments[0]);
        Label? parentLabel = !string.IsNullOrWhiteSpace(segments[1]) ? new Label(segments[1]) : null;

        return new CategoryToImport(label, parentLabel);
    }
}
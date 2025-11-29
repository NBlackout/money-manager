using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace Infra.Write;

public class CsvCategorizationRuleImporter : ICategorizationRuleImporter
{
    private const string CellSeparator = ",";

    public async Task<CategorizationRuleToImport[]> Parse(Stream content)
    {
        using StreamReader reader = new(content);
        string? header = await reader.ReadLineAsync();
        if (header == null)
            return [];

        List<CategorizationRuleToImport> categories = [];
        while (await reader.ReadLineAsync() is { } line)
            categories.Add(Parse(line));

        return [..categories];
    }

    private static CategorizationRuleToImport Parse(string line)
    {
        string[] cells = line.Split(CellSeparator);

        return new CategorizationRuleToImport(new Label(cells[0]), cells[1]);
    }
}
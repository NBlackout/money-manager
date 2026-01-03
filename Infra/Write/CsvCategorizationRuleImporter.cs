using App.Write.Model.ValueObjects;
using App.Write.Ports;
using Infra.Shared;

namespace Infra.Write;

public class CsvCategorizationRuleImporter(ICsvHelper csvHelper) : ICategorizationRuleImporter
{
    public async Task<CategorizationRuleToImport[]> Parse(Stream content)
    {
        string[][] lines = await csvHelper.Read(content);

        return lines.Select(Parse).ToArray();
    }

    private static CategorizationRuleToImport Parse(string[] cells) =>
        new(new Label(cells[0]), cells[1]);
}
using App.Write.Model.ValueObjects;
using App.Write.Ports;
using Infra.Shared;

namespace Infra.Write;

public class CsvCategoryImporter(ICsvHelper csvHelper) : ICategoryImporter
{
    public async Task<CategoryToImport[]> Parse(Stream content)
    {
        string[][] lines = await csvHelper.Read(content);

        return lines.Select(Parse).ToArray();
    }

    private static CategoryToImport Parse(string[] cells)
    {
        Label label = new(cells[0]);
        Label? parentLabel = !string.IsNullOrWhiteSpace(cells[1]) ? new Label(cells[1]) : null;

        return new CategoryToImport(label, parentLabel);
    }
}
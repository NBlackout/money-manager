using App.Read.Ports;
using Tooling;

namespace Infra.Read;

public class CsvCategorizationRuleExporter : ICategorizationRuleExporter
{
    private static readonly string LineSeparator = Environment.NewLine;
    private const string CellSeparator = ",";

    public Task<Stream> Export(CategorizationRuleSummaryPresentation[] categories)
    {
        string[] rows = [Headers(), ..categories.Select(Row)];
        string content = string.Join(LineSeparator, rows);

        return Task.FromResult(content.ToUtf8Stream());
    }

    private static string Headers() =>
        string.Join(CellSeparator, "Category", "Keywords");

    private static string Row(CategorizationRuleSummaryPresentation categorizationRule) =>
        string.Join(CellSeparator, categorizationRule.CategoryLabel, categorizationRule.Keywords);
}
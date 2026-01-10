using App.Read.Ports;
using Infra.Shared;

namespace Infra.Read;

public class CsvCategorizationRuleExporter(ICsvHelper csvHelper) : ICategorizationRuleExporter
{
    public async Task<Stream> Export(CategorizationRuleSummaryPresentation[] categories) =>
        await csvHelper.Write(Headers(), [..categories.Select(Row)]);

    private static string[] Headers() => ["Category", "Keywords", "Amount"];

    private static string[] Row(CategorizationRuleSummaryPresentation categorizationRule) =>
    [
        categorizationRule.CategoryLabel, categorizationRule.Keywords, categorizationRule.Amount?.ToString() ?? string.Empty
    ];
}
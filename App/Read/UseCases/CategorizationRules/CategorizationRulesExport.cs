using App.Read.Ports;

namespace App.Read.UseCases.CategorizationRules;

public class CategorizationRulesExport(ICategorizationRuleSummariesDataSource dataSource, ICategorizationRuleExporter exporter)
{
    public async Task<Stream> Execute()
    {
        CategorizationRuleSummaryPresentation[] categorizationRules = await dataSource.All();

        return await exporter.Export(categorizationRules);
    }
}
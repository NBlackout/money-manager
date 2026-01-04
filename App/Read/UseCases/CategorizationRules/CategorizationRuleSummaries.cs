using App.Read.Ports;

namespace App.Read.UseCases.CategorizationRules;

public class CategorizationRuleSummaries(ICategorizationRuleSummariesDataSource dataSource)
{
    public async Task<CategorizationRuleSummaryPresentation[]> Execute() =>
        await dataSource.All();
}
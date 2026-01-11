using App.Read.Ports;
using App.Write.Model.CategorizationRules;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryCategorizationRuleSummariesDataSource(
    InMemoryCategorizationRuleRepository categorizationRuleRepository,
    InMemoryCategoryRepository categoryRepository
) : ICategorizationRuleSummariesDataSource
{
    public Task<CategorizationRuleSummaryPresentation[]> All()
    {
        CategorizationRuleSummaryPresentation[] presentations = [..categorizationRuleRepository.Data.Select(this.ToPresentation)];

        return Task.FromResult(presentations);
    }

    private CategorizationRuleSummaryPresentation ToPresentation(CategorizationRuleSnapshot categorizationRule) =>
        new(
            categorizationRule.Id.Value,
            categorizationRule.CategoryId.Value,
            categoryRepository.Data.Single(c => c.Id == categorizationRule.CategoryId).Label,
            categorizationRule.Keywords,
            categorizationRule.Amount,
            categorizationRule.Margin
        );
}
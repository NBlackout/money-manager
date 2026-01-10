using App.Read.Ports;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryCategorizationRuleSummariesDataSource(
    InMemoryCategorizationRuleRepository categorizationRuleRepository,
    InMemoryCategoryRepository categoryRepository
) : ICategorizationRuleSummariesDataSource
{
    public Task<CategorizationRuleSummaryPresentation[]> All()
    {
        CategorizationRuleSummaryPresentation[] presentations =
        [
            ..categorizationRuleRepository.Data.Select(cr => new CategorizationRuleSummaryPresentation(
                    cr.Id.Value,
                    cr.CategoryId.Value,
                    categoryRepository.Data.Single(c => c.Id == cr.CategoryId).Label,
                    cr.Keywords,
                    cr.Amount
                )
            )
        ];

        return Task.FromResult(presentations);
    }
}
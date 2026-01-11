using App.Read.Ports;
using App.Write.Model.CategorizationRules;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryCategoriesWithKeywordsDataSource(
    InMemoryCategoryRepository categoryRepository,
    InMemoryCategorizationRuleRepository categorizationRuleRepository
) : ICategoriesWithKeywordsDataSource
{
    public Task<CategoryWithKeywords[]> All()
    {
        CategoryWithKeywords[] categoriesWithKeywords = [..categorizationRuleRepository.Data.Select(this.ToPresentation)];

        return Task.FromResult(categoriesWithKeywords);
    }

    private CategoryWithKeywords ToPresentation(CategorizationRuleSnapshot categorizationRule) =>
        new(
            categorizationRule.CategoryId.Value,
            categoryRepository.Data.Single(c => c.Id == categorizationRule.CategoryId).Label,
            categorizationRule.Keywords,
            categorizationRule.Amount,
            categorizationRule.Margin
        );
}
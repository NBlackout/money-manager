using App.Read.Ports;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryCategoriesWithKeywordsDataSource(
    InMemoryCategoryRepository categoryRepository,
    InMemoryCategorizationRuleRepository categorizationRuleRepository
) : ICategoriesWithKeywordsDataSource
{
    public Task<CategoryWithKeywords[]> All()
    {
        CategoryWithKeywords[] categoriesWithKeywords =
        [
            ..categorizationRuleRepository.Data.Select(cr => new CategoryWithKeywords(
                    cr.CategoryId.Value,
                    categoryRepository.Data.Single(c => c.Id == cr.CategoryId).Label,
                    cr.Keywords
                )
            )
        ];

        return Task.FromResult(categoriesWithKeywords);
    }
}
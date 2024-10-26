namespace Read.Infra.DataSources;

public class InMemoryCategoriesWithKeywordsDataSource(InMemoryCategoryRepository repository)
    : ICategoriesWithKeywordsDataSource
{
    public Task<CategoryWithKeywords[]> All()
    {
        CategoryWithKeywords[] categoriesWithKeywords =
        [
            ..repository.Data
                .Where(c => !string.IsNullOrWhiteSpace(c.Keywords))
                .Select(c => new CategoryWithKeywords(c.Id.Value, c.Label, c.Keywords))
        ];

        return Task.FromResult(categoriesWithKeywords);
    }
}
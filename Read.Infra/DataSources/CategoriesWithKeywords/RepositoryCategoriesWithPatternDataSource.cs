using Write.Infra.Repositories;

namespace Read.Infra.DataSources.CategoriesWithKeywords;

public class RepositoryCategoriesWithKeywordsDataSource : ICategoriesWithKeywordsDataSource
{
    private readonly InMemoryCategoryRepository repository;

    public RepositoryCategoriesWithKeywordsDataSource(InMemoryCategoryRepository repository)
    {
        this.repository = repository;
    }

    public Task<CategoryWithKeywords[]> Get()
    {
        CategoryWithKeywords[] categoriesWithKeywords = this.repository.Data
            .Where(c => !string.IsNullOrWhiteSpace(c.Keywords))
            .Select(c => new CategoryWithKeywords(c.Id, c.Label, c.Keywords))
            .ToArray();

        return Task.FromResult(categoriesWithKeywords);
    }
}
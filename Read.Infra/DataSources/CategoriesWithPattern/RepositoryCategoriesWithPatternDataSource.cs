using Write.Infra.Repositories;

namespace Read.Infra.DataSources.CategoriesWithPattern;

public class RepositoryCategoriesWithPatternDataSource : ICategoriesWithPatternDataSource
{
    private readonly InMemoryCategoryRepository repository;

    public RepositoryCategoriesWithPatternDataSource(InMemoryCategoryRepository repository)
    {
        this.repository = repository;
    }

    public Task<CategoryWithPattern[]> Get()
    {
        CategoryWithPattern[] categoriesWithPattern = this.repository.Data
            .Where(c => !string.IsNullOrWhiteSpace(c.Pattern))
            .Select(c => new CategoryWithPattern(c.Id, c.Label, c.Pattern))
            .ToArray();

        return Task.FromResult(categoriesWithPattern);
    }
}
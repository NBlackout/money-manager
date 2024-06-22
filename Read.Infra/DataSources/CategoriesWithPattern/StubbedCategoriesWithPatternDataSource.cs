namespace Read.Infra.DataSources.CategoriesWithPattern;

public class StubbedCategoriesWithPatternDataSource : ICategoriesWithPatternDataSource
{
    private readonly List<CategoryWithPattern> categoriesWithPattern = [];

    public Task<CategoryWithPattern[]> Get() =>
        Task.FromResult(this.categoriesWithPattern.ToArray());

    public void Feed(params CategoryWithPattern[] categoriesWithPattern) =>
        this.categoriesWithPattern.AddRange(categoriesWithPattern);
}
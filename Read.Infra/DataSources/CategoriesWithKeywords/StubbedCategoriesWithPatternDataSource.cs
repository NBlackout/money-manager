namespace Read.Infra.DataSources.CategoriesWithKeywords;

public class StubbedCategoriesWithKeywordsDataSource : ICategoriesWithKeywordsDataSource
{
    private readonly List<CategoryWithKeywords> categoriesWithKeywords = [];

    public Task<CategoryWithKeywords[]> All() =>
        Task.FromResult(this.categoriesWithKeywords.ToArray());

    public void Feed(params CategoryWithKeywords[] expected) =>
        this.categoriesWithKeywords.AddRange(expected);
}
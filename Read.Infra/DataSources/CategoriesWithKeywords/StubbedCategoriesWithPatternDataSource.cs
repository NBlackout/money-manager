namespace Read.Infra.DataSources.CategoriesWithKeywords;

public class StubbedCategoriesWithKeywordsDataSource : ICategoriesWithKeywordsDataSource
{
    private readonly List<CategoryWithKeywords> categoriesWithKeywords = [];

    public Task<CategoryWithKeywords[]> Get() =>
        Task.FromResult(this.categoriesWithKeywords.ToArray());

    public void Feed(params CategoryWithKeywords[] categoriesWithKeywords) =>
        this.categoriesWithKeywords.AddRange(categoriesWithKeywords);
}
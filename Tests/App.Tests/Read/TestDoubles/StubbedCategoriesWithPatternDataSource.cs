using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedCategoriesWithKeywordsDataSource : ICategoriesWithKeywordsDataSource
{
    private readonly List<CategoryWithKeywords> categoriesWithKeywords = [];

    public Task<CategoryWithKeywords[]> All() =>
        Task.FromResult(this.categoriesWithKeywords.ToArray());

    public void Feed(params CategoryWithKeywords[] categoriesWithKeywords) =>
        this.categoriesWithKeywords.AddRange(categoriesWithKeywords);
}
using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedCategoriesWithKeywordsDataSource : ICategoriesWithKeywordsDataSource
{
    private readonly List<CategoryWithKeywords> data = [];

    public Task<CategoryWithKeywords[]> All() =>
        Task.FromResult(this.data.ToArray());

    public void Feed(CategoryWithKeywords[] categories) =>
        this.data.AddRange(categories);
}
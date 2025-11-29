using App.Read.Ports;
using App.Write.Model.Categories;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryCategorySummariesDataSource(InMemoryCategoryRepository repository) : ICategorySummariesDataSource
{
    public Task<CategorySummaryPresentation[]> All()
    {
        IGrouping<CategoryId?, CategorySnapshot>[] categoriesByParent = repository.Data.GroupBy(d => d.ParentId).ToArray();
        if (categoriesByParent.Length == 0)
            return Task.FromResult(Array.Empty<CategorySummaryPresentation>());

        CategorySummaryPresentation[] presentations = categoriesByParent
            .Single(c => c.Key is null)
            .Select(c => PresentationFrom(c, categoriesByParent.SingleOrDefault(p => p.Key == c.Id)?.ToArray() ?? []))
            .ToArray();

        return Task.FromResult(presentations);
    }

    private static CategorySummaryPresentation PresentationFrom(CategorySnapshot category, CategorySnapshot[] childCategories) =>
        new(category.Id.Value, category.Label, PresentationsFrom(childCategories));

    private static ChildCategorySummaryPresentation[] PresentationsFrom(CategorySnapshot[] categories) =>
        categories.Select(a => new ChildCategorySummaryPresentation(a.Id.Value, a.Label)).ToArray();
}
using App.Write.Model.Categories;
using App.Write.Model.Exceptions;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace Infra.Write.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly Dictionary<CategoryId, CategorySnapshot> data = new();

    public IEnumerable<CategorySnapshot> Data => this.data.Values.Select(c => c);
    public Func<CategoryId> NextId { get; set; } = null!;

    public Task<CategoryId> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Category> By(CategoryId id) =>
        Task.FromResult(Category.From(this.data[id]));

    public Task<Dictionary<Label, Category?>> By(Label[] labels)
    {
        return Task.FromResult(
            labels.ToDictionary(
                l => l,
                l => this.data.Values.SingleOrDefault(c => c.Label == l.Value) != null ? Category.From(this.data.Values.Single(c => c.Label == l.Value)) : null
            )
        );
    }

    public Task EnsureUnique(Label label)
    {
        if (this.data.Values.Any(c => new Label(c.Label) == label))
            throw new DuplicateCategoryException();

        return Task.CompletedTask;
    }

    public Task Save(Category category)
    {
        this.data[category.Id] = category.Snapshot;

        return Task.CompletedTask;
    }

    public Task Delete(CategoryId id)
    {
        this.data.Remove(id);

        return Task.CompletedTask;
    }

    public void Feed(params CategorySnapshot[] categories) =>
        categories.ToList().ForEach(category => this.data[category.Id] = category);

    public bool Exists(CategoryId id) =>
        this.data.ContainsKey(id);
}
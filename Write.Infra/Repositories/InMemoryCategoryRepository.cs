using Write.App.Model.Exceptions;

namespace Write.Infra.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly Dictionary<Guid, CategorySnapshot> data = new();

    public IEnumerable<CategorySnapshot> Data => this.data.Values.Select(c => c);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Category> By(Guid id) =>
        Task.FromResult(Category.From(this.data[id]));

    public Task<Category?> ByOrDefault(string label)
    {
        CategorySnapshot? snapshot = this.data.Values.SingleOrDefault(a => a.Label == label);
        return Task.FromResult(snapshot != null ? Category.From(snapshot) : null);
    }

    public Task EnsureUnique(string label)
    {
        if (this.data.Values.Any(c => c.Label.ToLower().Trim() == label.ToLower().Trim()))
            throw new DuplicateCategoryException();

        return Task.CompletedTask;
    }

    public Task Save(Category category)
    {
        this.data[category.Id] = category.Snapshot;

        return Task.CompletedTask;
    }

    public Task Delete(Guid id)
    {
        this.data.Remove(id);

        return Task.CompletedTask;
    }

    public void Feed(params CategorySnapshot[] categories) =>
        categories.ToList().ForEach(category => this.data[category.Id] = category);

    public bool Exists(Guid id) =>
        this.data.ContainsKey(id);
}
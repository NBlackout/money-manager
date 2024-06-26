namespace Write.Infra.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly Dictionary<Guid, CategorySnapshot> data = new();

    public IEnumerable<CategorySnapshot> Data => this.data.Values.Select(c => c);

    public Task<Category> By(Guid id) =>
        Task.FromResult(Category.From(this.data[id]));

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

    public void Feed(params Category[] categories) =>
        categories.ToList().ForEach(category => this.data[category.Id] = category.Snapshot);

    public bool Exists(Guid id) =>
        this.data.ContainsKey(id);

    public void Clear() =>
        this.data.Clear();
}
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

    public Task<Category> By(string label) => 
        Task.FromResult(Category.From(this.data.Single(a => a.Value.Label == label).Value));

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

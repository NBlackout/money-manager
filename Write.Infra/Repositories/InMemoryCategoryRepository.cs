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

    public void Feed(params Category[] categories) =>
        categories.ToList().ForEach(category => this.data[category.Id] = category.Snapshot);

    public void Clear() =>
        this.data.Clear();
}
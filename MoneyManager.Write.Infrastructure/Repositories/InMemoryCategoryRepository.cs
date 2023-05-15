namespace MoneyManager.Write.Infrastructure.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly Dictionary<Guid, Category> data = new();

    public IEnumerable<CategorySnapshot> Data => this.data.Values.Select(c => c.Snapshot);

    public Task<Category> ById(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task Save(Category category)
    {
        CategorySnapshot snapshot = category.Snapshot;
        this.data[category.Id] = Category.From(snapshot);

        return Task.CompletedTask;
    }

    public void Feed(params Category[] categories) =>
        categories.ToList().ForEach(category => this.data.Add(category.Id, category));

    public void Clear()
    {
        this.data.Clear();
    }
}
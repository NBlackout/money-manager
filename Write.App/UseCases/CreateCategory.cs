namespace Write.App.UseCases;

public class CreateCategory
{
    private readonly ICategoryRepository repository;

    public CreateCategory(ICategoryRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, string label, string pattern)
    {
        Category category = new(id, label, pattern);
        await this.repository.Save(category);
    }
}
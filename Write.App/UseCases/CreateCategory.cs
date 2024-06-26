namespace Write.App.UseCases;

public class CreateCategory
{
    private readonly ICategoryRepository repository;

    public CreateCategory(ICategoryRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, string label, string keywords)
    {
        Category category = new(id, label, keywords);
        await this.repository.Save(category);
    }
}
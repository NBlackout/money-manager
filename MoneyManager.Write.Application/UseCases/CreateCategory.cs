namespace MoneyManager.Write.Application.UseCases;

public class CreateCategory
{
    private readonly ICategoryRepository repository;

    public CreateCategory(ICategoryRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, string label)
    {
        Category category = new(id, label);
        await this.repository.Save(category);
    }
}
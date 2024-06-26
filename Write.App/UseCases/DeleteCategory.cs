namespace Write.App.UseCases;

public class DeleteCategory
{
    private readonly ICategoryRepository repository;

    public DeleteCategory(ICategoryRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id) =>
        await this.repository.Delete(id);
}
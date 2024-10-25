namespace Write.App.UseCases;

public class DeleteCategory(ICategoryRepository repository)
{
    public async Task Execute(CategoryId id) =>
        await repository.Delete(id);
}
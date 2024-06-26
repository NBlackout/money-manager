namespace Write.App.UseCases;

public class DeleteCategory(ICategoryRepository repository)
{
    public async Task Execute(Guid id) =>
        await repository.Delete(id);
}
namespace Write.App.UseCases;

public class CreateCategory(ICategoryRepository repository)
{
    public async Task Execute(CategoryId id, Label label, string keywords)
    {
        await repository.EnsureUnique(label);
        Category category = new(id, label, keywords);
        await repository.Save(category);
    }
}
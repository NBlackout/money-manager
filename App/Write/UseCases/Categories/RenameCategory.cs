using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases.Categories;

public class RenameCategory(ICategoryRepository repository)
{
    public async Task Execute(CategoryId id, Label label)
    {
        Category category = await repository.By(id);
        category.Rename(label);
        await repository.Save(category);
    }
}
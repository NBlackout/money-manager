using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases.Categories;

public class CreateCategory(ICategoryRepository repository)
{
    public async Task Execute(CategoryId id, Label label, CategoryId? parentId)
    {
        await repository.EnsureUnique(label);
        Category category = new(id, label, parentId);
        await repository.Save(category);
    }
}
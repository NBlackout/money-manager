using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;

namespace App.Write.Ports;

public interface ICategoryRepository
{
    Task<CategoryId> NextIdentity();
    Task<Category> By(CategoryId id);
    Task<Dictionary<Label, Category?>> By(Label[] labels);
    Task EnsureUnique(Label label);
    Task Save(Category category);
    Task Save(Category[] categories);
    Task Delete(CategoryId id);
}
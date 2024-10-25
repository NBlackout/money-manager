namespace Write.App.Ports;

public interface ICategoryRepository
{
    Task<CategoryId> NextIdentity();
    Task<Category?> ByOrDefault(Label label);
    Task EnsureUnique(Label label);
    Task Save(Category category);
    Task Delete(CategoryId id);
}

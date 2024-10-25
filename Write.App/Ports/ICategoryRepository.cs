namespace Write.App.Ports;

public interface ICategoryRepository
{
    Task<Guid> NextIdentity();
    Task<Category?> ByOrDefault(string label);
    Task EnsureUnique(string label);
    Task Save(Category category);
    Task Delete(Guid id);
}

namespace Write.App.Ports;

public interface ICategoryRepository
{
    Task<Category> By(string label);
    Task Save(Category category);
    Task Delete(Guid id);
}

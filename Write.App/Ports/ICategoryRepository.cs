namespace Write.App.Ports;

public interface ICategoryRepository
{
    Task Save(Category category);
}
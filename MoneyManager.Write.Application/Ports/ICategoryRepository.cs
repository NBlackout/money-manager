namespace MoneyManager.Write.Application.Ports;

public interface ICategoryRepository
{
    Task Save(Category category);
}
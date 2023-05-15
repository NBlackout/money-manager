namespace MoneyManager.Client.Write.Application.Ports;

public interface ICategoryGateway
{
    Task Create(Guid id, string label);
}
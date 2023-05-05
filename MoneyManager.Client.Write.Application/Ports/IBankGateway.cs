namespace MoneyManager.Client.Write.Application.Ports;

public interface IBankGateway
{
    Task AssignName(Guid id, string name);
}
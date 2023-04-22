namespace MoneyManager.Client.Application.Write.Ports;

public interface IAccountGateway
{
    Task StopTracking(Guid id);
}
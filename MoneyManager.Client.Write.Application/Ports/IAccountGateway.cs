namespace MoneyManager.Client.Write.Application.Ports;

public interface IAccountGateway
{
    Task StopTracking(Guid id);
    Task ResumeTracking(Guid id);
}
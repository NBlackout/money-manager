namespace Client.Write.App.Ports;

public interface IAccountGateway
{
    Task StopTracking(Guid id);
    Task ResumeTracking(Guid id);
    Task AssignLabel(Guid id, string label);
}
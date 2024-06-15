namespace Client.Write.App.Ports;

public interface IBankGateway
{
    Task AssignName(Guid id, string name);
}
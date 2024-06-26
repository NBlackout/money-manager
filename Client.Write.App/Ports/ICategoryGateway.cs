namespace Client.Write.App.Ports;

public interface ICategoryGateway
{
    Task Create(Guid id, string label, string keywords);
    Task Delete(Guid id);
}
namespace Client.Write.App.Ports;

public interface IBankStatementGateway
{
    Task Upload(string fileName, string contentType, Stream stream);
}
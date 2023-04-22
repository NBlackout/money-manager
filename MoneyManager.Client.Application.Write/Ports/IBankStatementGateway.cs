namespace MoneyManager.Client.Application.Write.Ports;

public interface IBankStatementGateway
{
    Task Upload(string fileName, string contentType, Stream stream);
}
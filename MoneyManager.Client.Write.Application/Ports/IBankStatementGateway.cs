namespace MoneyManager.Client.Write.Application.Ports;

public interface IBankStatementGateway
{
    Task Upload(string fileName, string contentType, Stream stream);
}
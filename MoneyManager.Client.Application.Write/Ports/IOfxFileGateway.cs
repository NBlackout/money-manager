namespace MoneyManager.Client.Application.Write.Ports;

public interface IOfxFileGateway
{
    Task Upload(string fileName, string contentType, Stream stream);
}
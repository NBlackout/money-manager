using MoneyManager.Client.Application.Write.Ports;

namespace MoneyManager.Client.Application.Write.UseCases.OfxFile;

public class UploadOfxFile
{
    private readonly IOfxFileGateway gateway;

    public UploadOfxFile(IOfxFileGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(string fileName, string contentType, Stream stream) =>
        await this.gateway.Upload(fileName, contentType, stream);
}
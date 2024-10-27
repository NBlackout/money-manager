namespace Client.Write.App.UseCases;

public class UploadBankStatement(IBankStatementGateway gateway)
{
    public async Task Execute(string fileName, string contentType, Stream stream) =>
        await gateway.Upload(fileName, contentType, stream);
}
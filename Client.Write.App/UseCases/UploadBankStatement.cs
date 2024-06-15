namespace Client.Write.App.UseCases;

public class UploadBankStatement
{
    private readonly IBankStatementGateway gateway;

    public UploadBankStatement(IBankStatementGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(string fileName, string contentType, Stream stream) =>
        await this.gateway.Upload(fileName, contentType, stream);
}
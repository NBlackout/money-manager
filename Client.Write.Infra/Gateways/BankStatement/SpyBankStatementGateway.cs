namespace Client.Write.Infra.Gateways.BankStatement;

public class SpyBankStatementGateway : IBankStatementGateway
{
    public List<(string, string, Stream)> Calls { get; } = [];

    public Task Upload(string fileName, string contentType, Stream stream)
    {
        this.Calls.Add((fileName, contentType, stream));

        return Task.CompletedTask;
    }
}
namespace MoneyManager.Client.Infrastructure.Write.BankStatementGateway;

public class SpyBankStatementGateway : IBankStatementGateway
{
    public List<(string, string, Stream)> Calls { get; } = new();

    public Task Upload(string fileName, string contentType, Stream stream)
    {
        this.Calls.Add((fileName, contentType, stream));

        return Task.CompletedTask;
    }
}
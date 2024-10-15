using Client.Write.App.Ports;

namespace Client.Write.App.Tests.TestDoubles;

public class SpyBankStatementGateway : IBankStatementGateway
{
    public List<(string, string, Stream)> Calls { get; } = [];

    public Task Upload(string fileName, string contentType, Stream stream)
    {
        this.Calls.Add((fileName, contentType, stream));

        return Task.CompletedTask;
    }
}
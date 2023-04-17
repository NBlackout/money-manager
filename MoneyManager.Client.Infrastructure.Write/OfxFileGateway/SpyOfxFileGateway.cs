namespace MoneyManager.Client.Infrastructure.Write.OfxFileGateway;

public class SpyOfxFileGateway : IOfxFileGateway
{
    public List<(string, string, Stream)> Calls { get; } = new();

    public Task Upload(string fileName, string contentType, Stream stream)
    {
        this.Calls.Add((fileName, contentType, stream));

        return Task.CompletedTask;
    }
}
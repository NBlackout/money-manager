namespace MoneyManager.Client.Infrastructure.Write.Tests.TestDoubles;

internal class SpyHttpMessageHandler : HttpMessageHandler
{
    public List<(string, string)> Calls { get; } = new();

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        this.Calls.Add((request.RequestUri!.AbsoluteUri, await request.Content!.ReadAsStringAsync(cancellationToken)));

        return new HttpResponseMessage();
    }
}
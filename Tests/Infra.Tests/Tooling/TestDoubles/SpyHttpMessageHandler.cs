namespace Infra.Tests.Tooling.TestDoubles;

public class SpyHttpMessageHandler : HttpMessageHandler
{
    public List<(HttpMethod, string, string)> Calls { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.Calls.Add((request.Method, request.RequestUri!.AbsoluteUri, await ContentOf(request, cancellationToken)));

        return new HttpResponseMessage();
    }

    private static async Task<string> ContentOf(HttpRequestMessage request, CancellationToken cancellationToken) =>
        request.Content != null ? await request.Content!.ReadAsStringAsync(cancellationToken) : string.Empty;
}
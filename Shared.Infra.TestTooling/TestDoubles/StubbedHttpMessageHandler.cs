using System.Text.Json;

namespace Shared.Infra.TestTooling.TestDoubles;

public class StubbedHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, string> payloadByUrls = new();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string requestUrl = request.RequestUri!.AbsoluteUri;

        return Task.FromResult(new HttpResponseMessage { Content = new StringContent(this.payloadByUrls[requestUrl]) });
    }

    public void Feed(string requestUrl, object expected) =>
        this.payloadByUrls[requestUrl] = JsonSerializer.Serialize(expected);
}
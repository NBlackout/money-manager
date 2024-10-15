using System.Net.Http.Headers;

namespace Client.Write.Infra.Gateways;

public class HttpBankStatementGateway(HttpClient httpClient) : IBankStatementGateway
{
    public async Task Upload(string fileName, string contentType, Stream stream)
    {
        StreamContent fileContent = new(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        using MultipartFormDataContent content = new();
        content.Add(fileContent, "file", fileName);

        HttpResponseMessage response = await httpClient.PostAsync("accounts", content);
        response.EnsureSuccessStatusCode();
    }
}

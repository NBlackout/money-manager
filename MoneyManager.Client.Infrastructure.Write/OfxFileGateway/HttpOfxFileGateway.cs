using System.Net.Http.Headers;

namespace MoneyManager.Client.Infrastructure.Write.OfxFileGateway;

public class HttpOfxFileGateway : IOfxFileGateway
{
    private readonly HttpClient httpClient;

    public HttpOfxFileGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Upload(string fileName, string contentType, Stream stream)
    {
        StreamContent fileContent = new(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        using MultipartFormDataContent content = new();
        content.Add(fileContent, "file", fileName);

        HttpResponseMessage response = await this.httpClient.PostAsync("accounts", content);
        response.EnsureSuccessStatusCode();
    }
}
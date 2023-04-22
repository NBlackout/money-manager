using System.Net.Http.Headers;

namespace MoneyManager.Client.Infrastructure.Write.BankStatementGateway;

public class HttpBankStatementGateway : IBankStatementGateway
{
    private readonly HttpClient httpClient;

    public HttpBankStatementGateway(HttpClient httpClient)
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
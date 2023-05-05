namespace MoneyManager.Client.Write.Infrastructure.BankGateway;

public class HttpBankGateway : IBankGateway
{
    private readonly HttpClient httpClient;

    public HttpBankGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AssignName(Guid id, string name)
    {
        (await this.httpClient.PutAsJsonAsync($"banks/{id}/name", new AssignBankNameDto(name)))
            .EnsureSuccessStatusCode();
    }
}
namespace MoneyManager.Client.Write.Infrastructure.Gateways.Bank;

public class HttpBankGateway : IBankGateway
{
    private readonly HttpClient httpClient;

    public HttpBankGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AssignName(Guid id, string name)
    {
        (await this.httpClient.PutAsJsonAsync($"banks/{id}/name", new BankNameDto(name)))
            .EnsureSuccessStatusCode();
    }
}
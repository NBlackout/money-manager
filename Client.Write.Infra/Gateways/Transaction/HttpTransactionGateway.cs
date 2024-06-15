namespace Client.Write.Infra.Gateways.Transaction;

public class HttpTransactionGateway : ITransactionGateway
{
    private readonly HttpClient httpClient;

    public HttpTransactionGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AssignCategory(Guid transactionId, Guid categoryId)
    {
        (await this.httpClient.PutAsJsonAsync($"transactions/{transactionId}/category",
            new TransactionCategoryDto(categoryId))).EnsureSuccessStatusCode();
    }
}
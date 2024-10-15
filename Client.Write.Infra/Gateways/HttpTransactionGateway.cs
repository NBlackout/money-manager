namespace Client.Write.Infra.Gateways;

public class HttpTransactionGateway(HttpClient httpClient) : ITransactionGateway
{
    public async Task AssignCategory(Guid transactionId, Guid categoryId)
    {
        (await httpClient.PutAsJsonAsync($"transactions/{transactionId}/category",
            new TransactionCategoryDto(categoryId))).EnsureSuccessStatusCode();
    }
}

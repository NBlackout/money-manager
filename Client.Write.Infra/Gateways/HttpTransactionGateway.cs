namespace Client.Write.Infra.Gateways;

public class HttpTransactionGateway(HttpClient httpClient) : ITransactionGateway
{
    public async Task AssignCategory(Guid transactionId, Guid categoryId)
    {
        TransactionCategoryDto dto = new(categoryId);

        (await httpClient.PutAsJsonAsync($"transactions/{transactionId}/category", dto)).EnsureSuccessStatusCode();
    }
}
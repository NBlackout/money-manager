namespace Client.Write.Infra.Gateways;

public class HttpBudgetGateway(HttpClient httpClient) : IBudgetGateway
{
    public async Task Define(Guid id, string name, decimal amount, DateOnly beginDate) =>
        (await httpClient.PostAsJsonAsync("budgets", new BudgetDto(id, name, amount, beginDate)))
        .EnsureSuccessStatusCode();
}
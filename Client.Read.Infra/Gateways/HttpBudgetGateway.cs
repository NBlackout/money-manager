namespace Client.Read.Infra.Gateways;

public class HttpBudgetGateway(HttpClient httpClient) : IBudgetGateway
{
    public async Task<BudgetSummaryPresentation[]> Summaries() =>
        (await httpClient.GetFromJsonAsync<BudgetSummaryPresentation[]>("api/budgets"))!;
}
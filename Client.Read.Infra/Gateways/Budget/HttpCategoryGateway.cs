namespace Client.Read.Infra.Gateways.Budget;

public class HttpBudgetGateway(HttpClient httpClient) : IBudgetGateway
{
    public async Task<BudgetSummaryPresentation[]> Summaries() =>
        (await httpClient.GetFromJsonAsync<BudgetSummaryPresentation[]>("budgets"))!;
}
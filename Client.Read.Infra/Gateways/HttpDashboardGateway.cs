namespace Client.Read.Infra.Gateways;

public class HttpDashboardGateway(HttpClient httpClient) : IDashboardGateway
{
    public async Task<SlidingBalancesPresentation> SlidingBalances() =>
        (await httpClient.GetFromJsonAsync<SlidingBalancesPresentation>("api/dashboard/sliding-balances"))!;
}
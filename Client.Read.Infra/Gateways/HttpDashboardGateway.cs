namespace Client.Read.Infra.Gateways;

public class HttpDashboardGateway(HttpClient httpClient) : IDashboardGateway
{
    public async Task<SlidingAccountBalancesPresentation> SlidingAccountBalances() =>
        (await httpClient.GetFromJsonAsync<SlidingAccountBalancesPresentation>(
            "api/dashboard/sliding-account-balances"
        ))!;
}
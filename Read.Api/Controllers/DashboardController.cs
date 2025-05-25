namespace Read.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(SlidingAccountBalances slidingAccountBalances) : ControllerBase
{
    [HttpGet("sliding-account-balances")]
    public async Task<SlidingAccountBalancesPresentation> SlidingAccountBalances() =>
        await slidingAccountBalances.Execute();
}
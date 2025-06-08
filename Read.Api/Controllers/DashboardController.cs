namespace Read.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(SlidingBalances slidingBalances) : ControllerBase
{
    [HttpGet("sliding-balances")]
    public async Task<SlidingBalancesPresentation> SlidingBalances() =>
        await slidingBalances.Execute();
}
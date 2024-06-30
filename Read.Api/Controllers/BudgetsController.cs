namespace Read.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetsController(BudgetSummaries budgetSummaries) : ControllerBase
{
    [HttpGet]
    public async Task<BudgetSummaryPresentation[]> Summaries() =>
        await budgetSummaries.Execute();
}
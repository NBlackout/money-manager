namespace MoneyManager.Read.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategorySummaries categorySummaries;

    public CategoriesController(CategorySummaries categorySummaries)
    {
        this.categorySummaries = categorySummaries;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<CategorySummaryPresentation>> Summaries() =>
        await this.categorySummaries.Execute();
}
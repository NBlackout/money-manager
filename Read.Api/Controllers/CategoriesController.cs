namespace Read.Api.Controllers;

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
    public async Task<CategorySummaryPresentation[]> Summaries() =>
        await this.categorySummaries.Execute();
}
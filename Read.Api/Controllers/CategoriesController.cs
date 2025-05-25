namespace Read.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CategorySummaries categorySummaries) : ControllerBase
{
    [HttpGet]
    public async Task<CategorySummaryPresentation[]> Summaries() =>
        await categorySummaries.Execute();
}
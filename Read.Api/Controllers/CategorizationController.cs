namespace Read.Api.Controllers;

[ApiController]
[Route("api/categorization")]
public class CategorizationController(CategorizationSuggestions categorizationSuggestions) : ControllerBase
{
    [HttpGet]
    public async Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        await categorizationSuggestions.Execute();
}
namespace Read.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategorizationController(CategorizationSuggestions categorizationSuggestions) : ControllerBase
{
    [HttpGet]
    public async Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        await categorizationSuggestions.Execute();
}
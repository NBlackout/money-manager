namespace Read.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategorizationController : ControllerBase
{
    private readonly CategorizationSuggestions categorizationSuggestions;

    public CategorizationController(CategorizationSuggestions categorizationSuggestions)
    {
        this.categorizationSuggestions = categorizationSuggestions;
    }

    [HttpGet]
    public async Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        await this.categorizationSuggestions.Execute();
}
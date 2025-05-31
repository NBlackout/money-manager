namespace Client.Pages;

public partial class Categorization : ComponentBase
{
    private CategorizationSuggestionPresentation[]? suggestions;

    [Inject] private CategorizationSuggestions CategorizationSuggestions { get; set; } = null!;
    [Inject] private AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;

    protected override async Task OnParametersSetAsync() =>
        this.suggestions = await this.CategorizationSuggestions.Execute();

    private async Task ApproveAll()
    {
        foreach (CategorizationSuggestionPresentation suggestion in this.suggestions!)
            await this.Approve(suggestion);
    }

    private async Task Approve(CategorizationSuggestionPresentation suggestion)
    {
        await this.AssignTransactionCategory.Execute(suggestion.TransactionId, suggestion.CategoryId);
        this.suggestions = [..this.suggestions!.Where(s => s != suggestion)];
    }
}
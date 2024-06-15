namespace Client.Components;

public partial class TransactionLabelAssignment : ComponentBase
{
    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }
    [Parameter] public EventCallback<(Guid, string)> OnCategoryAssigned { get; set; }
    
    protected override async Task OnInitializedAsync() =>
        this.categories = await this.CategorySummaries.Execute();

    private async Task AssignLabel(Guid categoryId)
    {
        await this.AssignTransactionCategory.Execute(this.Id, categoryId);
        await this.OnCategoryAssigned.InvokeAsync((this.Id, this.categories!.Single(c => c.Id == categoryId).Label));
    }
}
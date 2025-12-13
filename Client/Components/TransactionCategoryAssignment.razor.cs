using App.Read.Ports;
using App.Read.UseCases;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.UseCases;

namespace Client.Components;

public partial class TransactionCategoryAssignment : ComponentBase
{
    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }
    [Parameter] public string Label { get; set; } = null!;
    [Parameter] public EventCallback<(Guid, string)> OnCategoryAssigned { get; set; }

    protected override async Task OnInitializedAsync() =>
        this.categories = await this.CategorySummaries.Execute();

    private async Task AssignCategory(Guid categoryId)
    {
        await this.AssignTransactionCategory.Execute(new TransactionId(this.Id), new CategoryId(categoryId));
        await this.OnCategoryAssigned.InvokeAsync((this.Id, this.categories!.Single(c => c.Id == categoryId).Label));
    }

    private void NavigateToCategoryCreation() =>
        this.NavigationManager.NavigateTo($"/categories/create?keywords={this.Label}");
}
using MoneyManager.Client.Read.Application.UseCases;

namespace MoneyManager.Client.Pages;

public partial class Categories : ComponentBase
{
    private IReadOnlyCollection<CategorySummaryPresentation>? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        this.categories = await this.CategorySummaries.Execute();
}
using App.Read.Ports;
using App.Read.UseCases.Categories;

namespace Client.Components;

public partial class CategoryPicker : ComponentBase
{
    private CategorySummaryPresentation[]? categories;

    [Inject] private CategorySummaries CategorySummaries { get; set; } = null!;

    [Parameter] public EventCallback<(Guid, string)> OnPicked { get; set; }

    protected override async Task OnInitializedAsync() =>
        this.categories = await this.CategorySummaries.Execute();

    private async Task Pick(Guid id)
    {
        string label = this.categories!.SingleOrDefault(c => c.Id == id)?.Label ?? this.categories!.SelectMany(c => c.Children).Single(c => c.Id == id).Label;

        await this.OnPicked.InvokeAsync((id, label));
    }
}
namespace Client.Pages;

public partial class Categories : ComponentBase
{
    private bool isCreating;
    private ElementReference labelElement;

    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public CreateCategory CreateCategory { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        this.categories = await this.CategorySummaries.Execute();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isCreating)
            await this.labelElement.FocusAsync();
    }

    private void ShowCategoryForm() =>
        this.isCreating = true;

    private void HideCategoryForm() =>
        this.isCreating = false;

    private async Task LabelChanged(ChangeEventArgs arg)
    {
        Guid id = Guid.NewGuid();
        string label = arg.Value!.ToString()!;
        await this.CreateCategory.Execute(id, label);

        this.categories = this.categories!.Prepend(new CategorySummaryPresentation(id, label)).ToArray();
        this.HideCategoryForm();
    }
}
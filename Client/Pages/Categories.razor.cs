namespace Client.Pages;

public partial class Categories : ComponentBase
{
    private bool isCreating;

    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public CreateCategory CreateCategory { get; set; } = null!;
    [SupplyParameterFromForm] public Category? Model { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.Model ??= new Category();
        this.categories = await this.CategorySummaries.Execute();
    }

    private void ShowCategoryForm() =>
        this.isCreating = true;

    private void HideCategoryForm() =>
        this.isCreating = false;

    private async Task Submit()
    {
        Guid id = Guid.NewGuid();
        string label = this.Model!.Label!;
        string pattern = this.Model!.Pattern!;
        await this.CreateCategory.Execute(id, label, pattern);

        this.categories = this.categories!.Prepend(new CategorySummaryPresentation(id, label, pattern)).ToArray();
        this.HideCategoryForm();
    }

    public class Category
    {
        public string? Label { get; set; }
        public string? Pattern { get; set; }
    }
}
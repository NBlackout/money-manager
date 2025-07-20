using App.Read.Ports;
using App.Read.UseCases;
using App.Write.Model.Categories;
using App.Write.UseCases;
using App.Write.Model.ValueObjects;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Categories : ComponentBase
{
    private bool isCreating;

    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public CreateCategory CreateCategory { get; set; } = null!;
    [Inject] public DeleteCategory DeleteCategory { get; set; } = null!;
    [Inject] public CategoriesExport CategoriesExport { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    [SupplyParameterFromQuery] public string? Keywords { get; set; }
    [SupplyParameterFromForm] public CategoryForm? Category { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.Category ??= new CategoryForm { Keywords = this.Keywords };
        this.isCreating = this.Keywords != null;
        this.categories = await this.CategorySummaries.Execute();
    }

    private void ShowCategoryForm() =>
        this.isCreating = true;

    private void HideCategoryForm() =>
        this.isCreating = false;

    private async Task Submit()
    {
        Guid id = Guid.NewGuid();
        string label = this.Category!.Label!;
        string keywords = this.Category!.Keywords!;
        await this.CreateCategory.Execute(new CategoryId(id), new Label(label), keywords);

        this.categories = [..this.categories!.Prepend(new CategorySummaryPresentation(id, label, keywords))];
        this.Category = new CategoryForm();
        this.HideCategoryForm();
    }

    private async Task Delete(CategorySummaryPresentation category)
    {
        await this.DeleteCategory.Execute(new CategoryId(category.Id));
        this.categories = [..this.categories!.Where(c => c != category)];
    }

    private async Task ExportCategories()
    {
        await using Stream content = await this.CategoriesExport.Execute();
        using DotNetStreamReference reference = new(content);

        await this.JsRuntime.InvokeVoidAsync("downloadFileFromStream", "categories.csv", reference);
    }

    public class CategoryForm
    {
        public string? Label { get; set; }
        public string? Keywords { get; set; }
    }
}
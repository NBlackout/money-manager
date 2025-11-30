using App.Read.Ports;
using App.Read.UseCases;
using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Categories : ComponentBase
{
    private const int OneMegaBytes = 1 * 1024 * 1024;

    private bool isCreating;
    private string? uploadResult;
    private InputText? labelElement;
    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public ImportCategories ImportCategories { get; set; } = null!;
    [Inject] public CreateCategory CreateCategory { get; set; } = null!;
    [Inject] public CategoriesExport CategoriesExport { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    [SupplyParameterFromQuery] public string? Keywords { get; set; }
    [SupplyParameterFromForm] public CategoryForm? Category { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.Category ??= new CategoryForm();
        this.isCreating = this.Keywords != null;
        await this.LoadCategories();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isCreating && this.labelElement?.Element != null)
            await this.labelElement.Element.Value.FocusAsync();
    }

    private async Task LoadCategories() =>
        this.SetCategoriesTo(await this.CategorySummaries.Execute());

    private void EnterCreateMode() =>
        this.isCreating = true;

    private void ExitCreateMode() =>
        this.isCreating = false;

    private async Task Submit()
    {
        Guid id = Guid.NewGuid();
        string label = this.Category!.Label!;
        await this.CreateCategory.Execute(new CategoryId(id), new Label(label), null);

        this.SetCategoriesTo([..this.categories!.Append(new CategorySummaryPresentation(id, label))]);
        this.Category = new CategoryForm();
        this.ExitCreateMode();
    }

    private async Task ExportCategories()
    {
        await using Stream content = await this.CategoriesExport.Execute();
        using DotNetStreamReference reference = new(content);

        await this.JsRuntime.InvokeVoidAsync("downloadFileFromStream", "categories.csv", reference);
    }

    private async Task ImportCategoriesFile(InputFileChangeEventArgs args)
    {
        try
        {
            await this.Upload(args.File);
            await this.LoadCategories();
            this.uploadResult = "Categories successfully imported";
        }
        catch (Exception e)
        {
            this.uploadResult = e.Message;
        }
    }

    private async Task Upload(IBrowserFile file)
    {
        await using MemoryStream buffer = new();
        await file.OpenReadStream(OneMegaBytes).CopyToAsync(buffer);
        buffer.Position = 0;

        await this.ImportCategories.Execute(buffer);
    }

    private void OnCategoryDeleted(Guid categoryId) =>
        this.SetCategoriesTo([..this.categories!.Where(c => c.Id != categoryId)]);

    private void SetCategoriesTo(CategorySummaryPresentation[] presentations) =>
        this.categories = presentations.OrderBy(p => p.Label).Select(p => p with { Children = p.Children.OrderBy(c => c.Label).ToArray() }).ToArray();

    public class CategoryForm
    {
        public string? Label { get; set; }
    }
}
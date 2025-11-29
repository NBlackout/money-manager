using App.Read.Ports;
using App.Read.UseCases;
using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.UseCases;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class CategorizationRules : ComponentBase
{
    private const int OneMegaBytes = 1 * 1024 * 1024;

    private bool isCreating;
    private string? uploadResult;

    private CategorizationRuleSummaryPresentation[]? categorizationRules;
    private CategorySummaryPresentation[]? categories;

    [Inject] public CategorizationRuleSummaries CategorizationRuleSummaries { get; set; } = null!;
    [Inject] public CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] public ImportCategorizationRules ImportCategorizationRules { get; set; } = null!;
    [Inject] public CreateCategorizationRule CreateCategorizationRule { get; set; } = null!;
    [Inject] public DeleteCategorizationRule DeleteCategorizationRule { get; set; } = null!;
    [Inject] public CategorizationRulesExport CategorizationRulesExport { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    [SupplyParameterFromQuery] public string? Keywords { get; set; }
    [SupplyParameterFromForm] public CategorizationRuleForm? CategorizationRule { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.CategorizationRule ??= new CategorizationRuleForm { Keywords = this.Keywords };
        this.isCreating = this.Keywords != null;
        this.categorizationRules = await this.CategorizationRuleSummaries.Execute();
        this.categories = await this.CategorySummaries.Execute();
    }

    private void ShowCategorizationRuleForm() =>
        this.isCreating = true;

    private void HideCategorizationRuleForm() =>
        this.isCreating = false;

    private async Task Submit()
    {
        Guid id = Guid.NewGuid();
        Guid categoryId = this.CategorizationRule!.CategoryId!.Value;
        string keywords = this.CategorizationRule!.Keywords!;
        await this.CreateCategorizationRule.Execute(new CategorizationRuleId(id), new CategoryId(categoryId), keywords);

        this.categorizationRules =
        [
            ..this.categorizationRules!.Prepend(
                new CategorizationRuleSummaryPresentation(id, categoryId, this.categories!.Single(c => c.Id == categoryId).Label, keywords)
            )
        ];
        this.CategorizationRule = new CategorizationRuleForm();
        this.HideCategorizationRuleForm();
    }

    private async Task Delete(CategorizationRuleSummaryPresentation categorizationRule)
    {
        await this.DeleteCategorizationRule.Execute(new CategorizationRuleId(categorizationRule.Id));
        this.categorizationRules = [..this.categorizationRules!.Where(c => c != categorizationRule)];
    }

    private async Task ExportCategorizationRules()
    {
        await using Stream content = await this.CategorizationRulesExport.Execute();
        using DotNetStreamReference reference = new(content);

        await this.JsRuntime.InvokeVoidAsync("downloadFileFromStream", "categorizationRules.csv", reference);
    }

    private async Task ImportCategorizationRulesFile(InputFileChangeEventArgs args)
    {
        try
        {
            await this.Upload(args.File);

            this.uploadResult = "Categorization rules successfully imported";
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

        await this.ImportCategorizationRules.Execute(buffer);
    }

    public class CategorizationRuleForm
    {
        public Guid? CategoryId { get; set; }
        public string? Keywords { get; set; }
    }
}
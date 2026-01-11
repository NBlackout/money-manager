using App.Read.Ports;
using App.Read.UseCases.Categories;
using App.Read.UseCases.CategorizationRules;
using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;
using App.Write.UseCases.CategorizationRules;
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

    [Inject] private CategorizationRuleSummaries CategorizationRuleSummaries { get; set; } = null!;
    [Inject] private CategorySummaries CategorySummaries { get; set; } = null!;
    [Inject] private ImportCategorizationRules ImportCategorizationRules { get; set; } = null!;
    [Inject] private ApplyCategorizationRule ApplyCategorizationRule { get; set; } = null!;
    [Inject] private DeleteCategorizationRule DeleteCategorizationRule { get; set; } = null!;
    [Inject] private CategorizationRulesExport CategorizationRulesExport { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [SupplyParameterFromForm] public CategorizationRuleForm? CategorizationRule { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.CategorizationRule ??= new CategorizationRuleForm();
        await this.LoadCategorizationRules();
        this.categories = await this.CategorySummaries.Execute();
    }

    private async Task LoadCategorizationRules() =>
        this.categorizationRules = await this.CategorizationRuleSummaries.Execute();

    private void EnterCreateMode() =>
        this.isCreating = true;

    private void HideCategorizationRuleForm() =>
        this.isCreating = false;

    private async Task Create()
    {
        if (this.CategorizationRule!.CategoryId.HasValue is false)
            return;
        if (string.IsNullOrWhiteSpace(this.CategorizationRule!.Keywords))
            return;

        Guid id = Guid.NewGuid();
        Guid categoryId = this.CategorizationRule!.CategoryId!.Value;
        string keywords = this.CategorizationRule!.Keywords!;
        decimal? amount = this.CategorizationRule!.Amount;
        decimal? margin = this.CategorizationRule!.Margin;
        await this.ApplyCategorizationRule.Execute(
            new CategorizationRuleId(id),
            new CategoryId(categoryId),
            keywords,
            amount.HasValue ? new Amount(amount.Value) : null,
            margin.HasValue ? new Amount(margin.Value) : null
        );

        string label = this.categories!.SingleOrDefault(c => c.Id == categoryId)?.Label ??
            this.categories!.SelectMany(c => c.Children).Single(c => c.Id == categoryId).Label;
        this.categorizationRules =
        [
            ..this.categorizationRules!.Prepend(new CategorizationRuleSummaryPresentation(id, categoryId, label, keywords, amount, margin))
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
            await this.LoadCategorizationRules();
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
        public decimal? Amount { get; set; }
        public decimal? Margin { get; set; }
    }
}
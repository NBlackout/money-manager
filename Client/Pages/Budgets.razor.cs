namespace Client.Pages;

public partial class Budgets : ComponentBase
{
    private bool isCreating;

    private BudgetSummaryPresentation[]? budgets;

    [Inject] public BudgetSummaries BudgetSummaries { get; set; } = null!;
    [Inject] public DefineBudget DefineBudget { get; set; } = null!;

    [SupplyParameterFromForm] public BudgetForm? Budget { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.Budget ??= new BudgetForm();
        this.budgets = await this.BudgetSummaries.Execute();
    }

    private void ShowBudgetForm() =>
        this.isCreating = true;

    private void HideBudgetForm() =>
        this.isCreating = false;

    private async Task Submit()
    {
        Guid id = Guid.NewGuid();
        string name = this.Budget!.Label!;
        decimal amount = this.Budget!.Amount!.Value;
        await this.DefineBudget.Execute(id, name, amount);

        this.budgets = this.budgets!.Prepend(new BudgetSummaryPresentation(id, name, amount)).ToArray();
        this.HideBudgetForm();
    }

    public class BudgetForm
    {
        public string? Label { get; set; }
        public decimal? Amount { get; set; }
    }
}
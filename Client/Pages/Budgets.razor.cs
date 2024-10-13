namespace Client.Pages;

public partial class Budgets : ComponentBase
{
    private bool isCreating;

    private BudgetSummaryPresentation[]? budgets;
    private AccountSummaryPresentation[]? accounts;

    [Inject] public BudgetSummaries BudgetSummaries { get; set; } = null!;
    [Inject] public DefineBudget DefineBudget { get; set; } = null!;
    [Inject] public AccountSummaries AccountSummaries { get; set; } = null!;

    [SupplyParameterFromForm] public BudgetForm? Budget { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.Budget ??= new BudgetForm { BeginDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1) };
        this.budgets = await this.BudgetSummaries.Execute();
        this.accounts = await this.AccountSummaries.Execute();
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
        DateOnly beginDate = this.Budget!.BeginDate!.Value;
        await this.DefineBudget.Execute(id, name, amount, beginDate);

        this.budgets = this.budgets!.Prepend(new BudgetSummaryPresentation(id, name, amount, beginDate, amount)).ToArray();
        this.HideBudgetForm();
    }

    public class BudgetForm
    {
        public string? Label { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? BeginDate { get; set; }
    }
}
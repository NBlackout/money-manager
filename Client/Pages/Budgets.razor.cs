using App.Read.Ports;
using App.Read.UseCases;
using App.Write.Model.Budgets;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;

namespace Client.Pages;

public partial class Budgets : ComponentBase
{
    private bool isCreating;

    private BudgetSummaryPresentation[]? budgets;
    private AccountSummaryPresentation[]? accounts;

    [Inject] private BudgetSummaries BudgetSummaries { get; set; } = null!;
    [Inject] private DefineBudget DefineBudget { get; set; } = null!;
    [Inject] private AccountSummaries AccountSummaries { get; set; } = null!;

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
        await this.DefineBudget.Execute(new BudgetId(id), new Label(name), new Amount(amount), beginDate);

        this.budgets = [..this.budgets!.Prepend(new BudgetSummaryPresentation(id, name, amount, beginDate, amount))];
        this.Budget = new BudgetForm { BeginDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1) };
        this.HideBudgetForm();
    }

    public class BudgetForm
    {
        public string? Label { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? BeginDate { get; set; }
    }
}
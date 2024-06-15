using static System.DateTime;

namespace Client.Components;

public partial class AccountActivity : ComponentBase
{
    private bool isEditing;
    private ElementReference labelElement;

    private List<DateTime> months = new();
    private DateTime currentMonth;
    private AccountDetailsPresentation? account;
    private IReadOnlyCollection<TransactionSummaryPresentation>? transactions;

    [Inject] private AccountDetails AccountDetails { get; set; } = null!;
    [Inject] private TransactionsOfMonth TransactionsOfMonth { get; set; } = null!;
    [Inject] private AssignAccountLabel AssignAccountLabel { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        this.months = LoadMonthsRange();
        this.account = await this.AccountDetails.Execute(this.Id);
        await this.LoadTransactionsOf(new DateTime(Today.Year, Today.Month, 1));
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isEditing)
            await this.labelElement.FocusAsync();
    }

    private void ToggleEditMode() =>
        this.isEditing = true;

    private void ExitEditMode() =>
        this.isEditing = false;

    private async Task LabelChanged(ChangeEventArgs args)
    {
        string newLabel = (string)args.Value!;
        await this.AssignAccountLabel.Execute(this.account!.Id, newLabel);

        this.account = this.account! with { Label = newLabel };
        this.ExitEditMode();
    }

    private void OnCategoryAssigned((Guid TransactionId, string CategoryLabel) args)
    {
        TransactionSummaryPresentation transaction = this.transactions!.Single(t => t.Id == args.TransactionId);

        List<TransactionSummaryPresentation> updatedTransactions = this.transactions!.ToList();
        int transactionIndex = updatedTransactions.IndexOf(transaction);
        updatedTransactions[transactionIndex] = transaction with { Category = args.CategoryLabel };
        this.transactions = updatedTransactions;
    }

    private async Task ShowTransactionsOfMonth(ChangeEventArgs args) =>
        await this.LoadTransactionsOf(ParseExact(args.ToString()!, "MMMM yyyy", null));

    private async Task ShowFirstMonthTransactions() =>
        await this.LoadTransactionsOf(this.months.First());

    private async Task ShowPreviousMonthTransactions() =>
        await this.LoadTransactionsOf(this.currentMonth.AddMonths(-1));

    private async Task ShowNextMonthTransactions() =>
        await this.LoadTransactionsOf(this.currentMonth.AddMonths(1));

    private async Task ShowLastMonthTransactions() =>
        await this.LoadTransactionsOf(this.months.Last());

    private async Task LoadTransactionsOf(DateTime month)
    {
        this.currentMonth = month;
        this.transactions =
            await this.TransactionsOfMonth.Execute(this.Id, this.currentMonth.Year, this.currentMonth.Month);
    }

    private static List<DateTime> LoadMonthsRange()
    {
        DateTime januaryLastYear = new(Today.Year - 1, 1, 1);
        DateTime nextMonth = Today.AddMonths(1);

        List<DateTime> months = new();
        for (DateTime date = januaryLastYear; date < nextMonth; date = date.AddMonths(1))
            months.Add(date);

        return months;
    }
}
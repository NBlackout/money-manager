namespace Client.Components;

public partial class AccountActivity : ComponentBase
{
    private bool isEditing;
    private ElementReference labelElement;

    private List<DateOnly> months = [];
    private DateOnly currentMonth;
    private AccountDetailsPresentation? account;

    [Inject] private AccountDetails AccountDetails { get; set; } = null!;
    [Inject] private AssignAccountLabel AssignAccountLabel { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        this.months = LoadMonthsRange();
        this.currentMonth = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1);
        this.account = await this.AccountDetails.Execute(this.Id);
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
        string newLabel = (string) args.Value!;
        await this.AssignAccountLabel.Execute(this.account!.Id, newLabel);

        this.account = this.account! with { Label = newLabel };
        this.ExitEditMode();
    }

    private void ShowTransactionsOfMonth(ChangeEventArgs args) =>
        this.currentMonth = DateOnly.ParseExact(args.Value!.ToString()!, "yyyy-MM-dd", null);

    private void ShowFirstMonthTransactions() =>
        this.currentMonth = this.months.First();

    private void ShowPreviousMonthTransactions() =>
        this.currentMonth = this.currentMonth.AddMonths(-1);

    private void ShowNextMonthTransactions() =>
        this.currentMonth = this.currentMonth.AddMonths(+1);

    private void ShowLastMonthTransactions() =>
        this.currentMonth = this.months.Last();

    private static List<DateOnly> LoadMonthsRange()
    {
        DateOnly januaryLastYear = new(DateTime.Today.Year - 1, 1, 1);
        DateOnly nextMonth = DateOnly.FromDateTime(DateTime.Today).AddMonths(1);

        List<DateOnly> months = [];
        for (DateOnly date = januaryLastYear; date < nextMonth; date = date.AddMonths(1))
            months.Add(date);

        return months;
    }
}
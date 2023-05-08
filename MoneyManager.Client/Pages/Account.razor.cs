using static System.DateTime;

namespace MoneyManager.Client.Pages;

public partial class Account : ComponentBase
{
    private List<DateTime>? months;
    private DateTime currentMonth;
    private AccountDetailsPresentation? account;
    private IReadOnlyCollection<TransactionSummaryPresentation>? transactions;

    [Inject] private AccountDetails AccountDetails { get; set; } = null!;
    [Inject] private TransactionsOfMonth TransactionsOfMonth { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.currentMonth = new DateTime(Today.Year, Today.Month, 1);
        this.months = LoadMonthsRange();
        this.account = await this.AccountDetails.Execute(this.Id);
        this.transactions = await this.LoadTransactionsOfMonth(this.currentMonth);
    }

    private async Task ShowTransactionsOfMonth(ChangeEventArgs args) =>
        this.transactions = await this.LoadTransactionsOfMonth(ParseExact(args.Value!.ToString()!, "MMMM yyyy", null));

    private static List<DateTime> LoadMonthsRange()
    {
        DateTime januaryLastYear = new(Today.Year - 1, 1, 1);
        DateTime nextMonth = Today.AddMonths(1);

        List<DateTime> months = new();
        for (DateTime date = januaryLastYear; date < nextMonth; date = date.AddMonths(1))
            months.Add(date);

        return months;
    }

    private async Task<IReadOnlyCollection<TransactionSummaryPresentation>> LoadTransactionsOfMonth(DateTime today) =>
        await this.TransactionsOfMonth.Execute(this.Id, today.Year, today.Month);
}
namespace Client.Components;

public partial class Transactions : ComponentBase
{
    private TransactionSummaryPresentation[]? transactions;

    [Inject] private TransactionsOfMonth TransactionsOfMonth { get; set; } = null!;
    private TransactionSummaryPresentation[]? Inflow => this.transactions?.Where(t => t.Amount > 0).ToArray();
    private TransactionSummaryPresentation[]? Outflow => this.transactions?.Where(t => t.Amount <= 0).ToArray();

    [Parameter] public Guid AccountId { get; set; }
    [Parameter] public DateTime Month { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        this.transactions = await this.TransactionsOfMonth.Execute(this.AccountId, this.Month.Year, this.Month.Month);
    }

    private void OnCategoryAssigned((Guid TransactionId, string CategoryLabel) args)
    {
        TransactionSummaryPresentation transaction = this.transactions!.Single(t => t.Id == args.TransactionId);

        TransactionSummaryPresentation[] updatedTransactions = this.transactions!.ToArray();
        int transactionIndex = updatedTransactions.ToList().IndexOf(transaction);
        updatedTransactions[transactionIndex] = transaction with { Category = args.CategoryLabel };
        this.transactions = updatedTransactions;
    }
}

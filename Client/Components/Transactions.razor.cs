namespace Client.Components;

public partial class Transactions : ComponentBase
{
    private IReadOnlyCollection<TransactionSummaryPresentation>? transactions;

    [Inject] private TransactionsOfMonth TransactionsOfMonth { get; set; } = null!;

    [Parameter] public Guid AccountId { get; set; }
    [Parameter] public DateTime Month { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        this.transactions = await this.TransactionsOfMonth.Execute(this.AccountId, this.Month.Year, this.Month.Month);
    }

    private void OnCategoryAssigned((Guid TransactionId, string CategoryLabel) args)
    {
        TransactionSummaryPresentation transaction = this.transactions!.Single(t => t.Id == args.TransactionId);

        List<TransactionSummaryPresentation> updatedTransactions = this.transactions!.ToList();
        int transactionIndex = updatedTransactions.IndexOf(transaction);
        updatedTransactions[transactionIndex] = transaction with { Category = args.CategoryLabel };
        this.transactions = updatedTransactions;
    }
}
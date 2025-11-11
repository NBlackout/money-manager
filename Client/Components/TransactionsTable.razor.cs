using App.Read.Ports;
using App.Write.Model.Transactions;
using App.Write.UseCases;

namespace Client.Components;

public partial class TransactionsTable : ComponentBase
{
    [Inject] private ToggleTransactionRecurrence ToggleTransactionRecurrence { get; set; } = null!;

    [Parameter] public TransactionSummaryPresentation[] Transactions { get; set; } = null!;

    private decimal Total => this.Transactions.Sum(t => t.Amount);

    private void OnCategoryAssigned((Guid TransactionId, string CategoryLabel) args) =>
        this.Transactions = this.Transactions.Select(t => t with { Category = t.Id == args.TransactionId ? args.CategoryLabel : t.Category }).ToArray();

    private async Task ToggleRecurrence(Guid transactionId)
    {
        await this.ToggleTransactionRecurrence.Execute(new TransactionId(transactionId));

        this.Transactions = this.Transactions.Select(t => t with { IsRecurring = t.Id == transactionId ? !t.IsRecurring : t.IsRecurring }).ToArray();
    }
}
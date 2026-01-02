using App.Read.Ports;
using App.Write.Model.RecurringTransactions;
using App.Write.Model.Transactions;
using App.Write.UseCases;

namespace Client.Components;

public partial class TransactionsTable : ComponentBase
{
    [Inject] private AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;
    [Inject] private MarkTransactionAsRecurring MarkTransactionAsRecurring { get; set; } = null!;

    [Parameter] public TransactionSummaryPresentation[] Transactions { get; set; } = null!;
    [Parameter] public EventCallback<Guid> OnPicked { get; set; }

    private decimal Total => this.Transactions.Sum(t => t.Amount);

    private async Task MarkAsRecurring(Guid transactionId)
    {
        await this.MarkTransactionAsRecurring.Execute(new TransactionId(transactionId), new RecurringTransactionId(Guid.NewGuid()));

        this.Transactions = this.Transactions.Select(t => t with { IsRecurring = t.Id == transactionId ? !t.IsRecurring : t.IsRecurring }).ToArray();
    }

    private async Task OpenCategoryPickerFor(Guid transactionId) =>
        await this.OnPicked.InvokeAsync(transactionId);
}
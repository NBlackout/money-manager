using App.Read.Ports;

namespace Client.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public TransactionSummaryPresentation[] Transactions { get; set; } = null!;
    [Parameter] public EventCallback<Guid> OnPicked { get; set; }

    private async Task OpenCategoryPickerFor(Guid transactionId) =>
        await this.OnPicked.InvokeAsync(transactionId);
}
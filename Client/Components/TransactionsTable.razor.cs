using App.Read.Ports;
using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;
using App.Write.UseCases.Transactions;

namespace Client.Components;

public partial class TransactionsTable : ComponentBase
{
    private bool isEditing;
    private Guid? transactionId;
    private ElementReference labelElement;

    [Inject] private PreferTransactionLabel PreferTransactionLabel { get; set; } = null!;

    [Parameter] public TransactionSummaryPresentation[] Transactions { get; set; } = null!;
    [Parameter] public EventCallback<Guid> OnPicked { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isEditing)
            await this.labelElement.FocusAsync();
    }

    private async Task LabelChanged(ChangeEventArgs args)
    {
        string newLabel = (string)args.Value!;
        await this.PreferTransactionLabel.Execute(new TransactionId(this.transactionId!.Value), new Label(newLabel));

        this.Transactions = this.Transactions.Select(t => t with { Label = t.Id == this.transactionId.Value ? newLabel : t.Label }).ToArray();
        this.ExitEditMode();
    }

    private void EnterEditMode(Guid transactionId)
    {
        this.isEditing = true;
        this.transactionId = transactionId;
    }

    private void ExitEditMode()
    {
        this.isEditing = false;
        this.transactionId = null;
    }

    private async Task OpenCategoryPickerFor(Guid transactionId) =>
        await this.OnPicked.InvokeAsync(transactionId);
}
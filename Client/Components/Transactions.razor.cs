using App.Read.Ports;
using App.Read.UseCases.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.UseCases.Transactions;
using Microsoft.JSInterop;

namespace Client.Components;

public partial class Transactions : ComponentBase
{
    private TransactionSummaryPresentation[]? transactions;
    private Guid? selectedTransactionId;

    [Inject] private TransactionsOfMonth TransactionsOfMonth { get; set; } = null!;
    [Inject] private AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public Guid AccountId { get; set; }
    [Parameter] public DateOnly Month { get; set; }

    private TransactionSummaryPresentation[]? Inflow => this.transactions?.Where(t => t.Amount > 0).ToArray();
    private TransactionSummaryPresentation[]? Outflow => this.transactions?.Where(t => t.Amount <= 0).ToArray();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await this.JsRuntime.InvokeVoidAsync("hookOffcanvasTo", "#categoryPickerOffcanvas");
    }

    protected override async Task OnParametersSetAsync() =>
        this.transactions = await this.TransactionsOfMonth.Execute(this.AccountId, this.Month.Year, this.Month.Month);

    private async Task OnCategoryPicked((Guid CategoryId, string CategoryLabel) args)
    {
        await this.AssignTransactionCategory.Execute(new TransactionId(this.selectedTransactionId!.Value), new CategoryId(args.CategoryId));
        this.transactions = this.transactions!
            .Select(t => t with { Category = t.Id == this.selectedTransactionId!.Value ? args.CategoryLabel : t.Category })
            .ToArray();
        await this.UnselectTransaction();
    }

    private async Task OnCategoryPickerClosed() =>
        await this.UnselectTransaction();

    private async Task OpenCategoryPickerFor(Guid transactionId) =>
        await this.SelectTransaction(transactionId);

    private async Task UnselectTransaction()
    {
        await this.JsRuntime.InvokeVoidAsync("hideOffcanvas");
        this.selectedTransactionId = null;
    }

    private async Task SelectTransaction(Guid transactionId)
    {
        await this.JsRuntime.InvokeVoidAsync("showOffcanvas");
        this.selectedTransactionId = transactionId;
    }
}
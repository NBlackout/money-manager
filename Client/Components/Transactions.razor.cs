using App.Read.Ports;
using App.Read.UseCases.Accounts;
using Microsoft.JSInterop;

namespace Client.Components;

public partial class Transactions : ComponentBase
{
    private TransactionSummaryPresentation[]? transactions;

    [Inject] private TransactionsOfMonth TransactionsOfMonth { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private Store Store { get; set; } = null!;

    [Parameter] public Guid AccountId { get; set; }
    [Parameter] public DateOnly Month { get; set; }

    private TransactionSummaryPresentation[]? Inflow => this.transactions?.Where(t => t.Amount > 0).ToArray();
    private TransactionSummaryPresentation[]? Outflow => this.transactions?.Where(t => t.Amount <= 0).ToArray();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await this.JsRuntime.InvokeVoidAsync("hookOffcanvasTo", "#transactionDetails");
    }

    protected override async Task OnParametersSetAsync() =>
        this.transactions = await this.TransactionsOfMonth.Execute(this.AccountId, this.Month.Year, this.Month.Month);

    private async Task OnCategoryPicked((Guid CategoryId, string CategoryLabel) args)
    {
        this.transactions = this.transactions!
            .Select(t => t with { Category = t.Id == this.Store.SelectedTransaction!.Id ? args.CategoryLabel : t.Category })
            .ToArray();
        await this.UnselectTransaction();
    }

    private async Task OnDetailsClosed() =>
        await this.UnselectTransaction();

    private async Task OpenDetailsOf(Guid transactionId) =>
        await this.SelectTransaction(this.transactions!.Single(t => t.Id == transactionId));

    private async Task UnselectTransaction()
    {
        await this.JsRuntime.InvokeVoidAsync("hideOffcanvas");
        this.Store.SelectedTransaction = null;
    }

    private async Task SelectTransaction(TransactionSummaryPresentation transaction)
    {
        await this.JsRuntime.InvokeVoidAsync("showOffcanvas");
        this.Store.SelectedTransaction = transaction;
    }
}
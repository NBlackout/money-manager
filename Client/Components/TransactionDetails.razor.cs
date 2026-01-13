using App.Read.Ports;
using App.Write.Model.Categories;
using App.Write.Model.RecurringTransactions;
using App.Write.Model.Transactions;
using App.Write.UseCases.Transactions;

namespace Client.Components;

public partial class TransactionDetails : ComponentBase
{
    private bool pickCategory;
    private TransactionSummaryPresentation? Transaction => this.Store.SelectedTransaction;

    [Inject] private AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;
    [Inject] private MarkTransactionAsRecurring MarkTransactionAsRecurring { get; set; } = null!;
    [Inject] private Store Store { get; set; } = null!;

    [Parameter] public EventCallback<(Guid, string)> OnPicked { get; set; }
    [Parameter] public EventCallback OnClosed { get; set; }

    private async Task OnCategoryPicked((Guid CategoryId, string CategoryLabel) args)
    {
        await this.AssignTransactionCategory.Execute(new TransactionId(this.Transaction!.Id), new CategoryId(args.CategoryId));
        this.Store.SelectedTransaction = this.Store.SelectedTransaction! with { Category = args.CategoryLabel };
        this.pickCategory = false;
    }

    private async Task MarkAsRecurring()
    {
        await this.MarkTransactionAsRecurring.Execute(new TransactionId(this.Transaction!.Id), new RecurringTransactionId(Guid.NewGuid()));
        this.Store.SelectedTransaction = this.Store.SelectedTransaction! with { IsRecurring = true };
    }

    private void PickCategory() =>
        this.pickCategory = true;
}
using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.UseCases.Transactions;

namespace Client.Components;

public partial class TransactionDetails : ComponentBase
{
    private bool pickCategory;

    [Inject] private AssignTransactionCategory AssignTransactionCategory { get; set; } = null!;
    [Inject] private Store Store { get; set; } = null!;

    [Parameter] public EventCallback<(Guid, string)> OnPicked { get; set; }
    [Parameter] public EventCallback OnClosed { get; set; }

    private async Task OnCategoryPicked((Guid CategoryId, string CategoryLabel) args)
    {
        await this.AssignTransactionCategory.Execute(new TransactionId(this.Store.SelectedTransactionId!.Value), new CategoryId(args.CategoryId));
        await this.OnPicked.InvokeAsync(args);
        this.pickCategory = false;
    }

    private void PickCategory() =>
        this.pickCategory = true;
}
using App.Read.Ports;
using App.Write.Model.Categories;
using App.Write.Model.RecurringTransactions;

namespace App.Tests.Read.Tooling;

public record RecurringTransactionBuilder(Guid Id, decimal Amount, string Label, DateOnly Date, CategoryBuilder? Category)
{
    public RecurringTransactionSnapshot ToSnapshot() =>
        new(new RecurringTransactionId(this.Id), this.Amount, this.Label, this.Date, this.Category != null ? new CategoryId(this.Category!.Id) : null);

    public ExpectedTransactionSummaryPresentation ToSummary() =>
        new(this.Date, this.Amount, this.Label, this.Category?.Label);
}
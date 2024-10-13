using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Transactions;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record TransactionBuilder(
    Guid Id,
    Guid AccountId,
    decimal Amount,
    string Label,
    DateTime Date,
    CategoryBuilder? Category)
{
    public TransactionSnapshot ToSnapshot() =>
        new(this.Id, this.AccountId, "External id", this.Amount, this.Label, this.Date, this.Category?.Id);

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date, this.Category?.Label);
}
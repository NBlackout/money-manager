using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Accounts;
using Write.App.Model.Categories;
using Write.App.Model.Transactions;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record TransactionBuilder(
    Guid Id,
    Guid AccountId,
    decimal Amount,
    string Label,
    DateOnly Date,
    CategoryBuilder? Category)
{
    public TransactionSnapshot ToSnapshot()
    {
        return new TransactionSnapshot(
            new TransactionId(this.Id),
            new AccountId(this.AccountId),
            "External id",
            this.Amount,
            this.Label,
            this.Date,
            this.Category is not null ? new CategoryId(this.Category.Id) : null
        );
    }

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date, this.Category?.Label);
}
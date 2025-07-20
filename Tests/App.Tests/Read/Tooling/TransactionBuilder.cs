using System.Diagnostics.CodeAnalysis;
using App.Read.Ports;
using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;

namespace App.Tests.Read.Tooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record TransactionBuilder(Guid Id, Guid AccountId, decimal Amount, string Label, DateOnly Date, CategoryBuilder? Category)
{
    public TransactionSnapshot ToSnapshot() =>
        new(
            new TransactionId(this.Id),
            new AccountId(this.AccountId),
            "External id",
            this.Amount,
            this.Label,
            this.Date,
            this.Category is not null ? new CategoryId(this.Category.Id) : null
        );

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date, this.Category?.Label);
}
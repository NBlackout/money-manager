using App.Read.Ports;
using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;

namespace App.Tests.Read.Tooling;

public record TransactionBuilder(Guid Id, Guid AccountId, decimal Amount, string Label, DateOnly Date, CategoryBuilder? Category, bool IsRecurring)
{
    public TransactionSnapshot ToSnapshot() =>
        new(
            new TransactionId(this.Id),
            new AccountId(this.AccountId),
            "External id",
            this.Amount,
            this.Label,
            this.Date,
            this.Category is not null ? new CategoryId(this.Category.Id) : null,
            this.IsRecurring
        );

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date, this.Category?.Label, this.IsRecurring);
}
using System.Diagnostics.CodeAnalysis;
using Shared.Presentation;
using Write.App.Model.Accounts;

namespace Read.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record AccountBuilder(Guid Id, string Label, string Number, decimal Balance, DateTime BalanceDate)
{
    public AccountSnapshot ToSnapshot() =>
        new(this.Id, this.Number, this.Label, this.Balance, this.BalanceDate);

    public AccountSummaryPresentation ToSummary() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);

    public AccountDetailsPresentation ToDetails() =>
        new(this.Id, this.Label, this.Number, this.Balance, this.BalanceDate);
}
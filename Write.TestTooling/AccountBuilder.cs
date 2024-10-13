using System.Diagnostics.CodeAnalysis;
using Write.App.Model.Accounts;

namespace Write.TestTooling;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record AccountBuilder(Guid Id, string Number, string Label, decimal Balance, DateTime BalanceDate)
{
    public Account Build() =>
        Account.From(this.ToSnapshot());

    public AccountSnapshot ToSnapshot() =>
        new(this.Id, this.Number, this.Label, this.Balance, this.BalanceDate);
}

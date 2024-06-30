using Write.App.Model.Accounts;

namespace Write.TestTooling;

public record AccountBuilder(
    Guid Id,
    Guid BankId,
    string Number,
    string Label,
    decimal Balance,
    DateTime BalanceDate)
{
    public Account Build() =>
        Account.From(this.ToSnapshot());

    public AccountSnapshot ToSnapshot() =>
        new(this.Id, this.BankId, this.Number, this.Label, this.Balance, this.BalanceDate);
}

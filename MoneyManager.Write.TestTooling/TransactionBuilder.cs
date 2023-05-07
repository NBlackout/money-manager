using MoneyManager.Write.Application.Model.Transactions;

namespace MoneyManager.Write.TestTooling;

public record TransactionBuilder(Guid Id, Guid AccountId, string ExternalId, decimal Amount, string Label, DateTime Date)
{
    public static TransactionBuilder For(Guid id) =>
        new(id, Guid.Parse("B7675755-4E2D-49D3-9E7E-D1CF1EA6A84C"), "External id", 552.75m, "The label", DateTime.Parse("2023-04-19"));

    public Transaction Build() =>
        Transaction.From(new TransactionSnapshot(this.Id, this.AccountId, this.ExternalId, this.Amount, this.Label, this.Date));
}
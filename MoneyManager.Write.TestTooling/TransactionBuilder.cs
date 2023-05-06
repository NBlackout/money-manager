using MoneyManager.Write.Application.Model.Transactions;

namespace MoneyManager.Write.TestTooling;

public record TransactionBuilder(Guid Id, Guid AccountId, string ExternalId)
{
    public static TransactionBuilder For(Guid id) =>
        new(id, Guid.Parse("B7675755-4E2D-49D3-9E7E-D1CF1EA6A84C"), "External id");

    public Transaction Build() =>
        Transaction.From(new TransactionSnapshot(this.Id, this.AccountId, this.ExternalId));
}
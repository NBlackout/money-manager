using Write.App.Model.Transactions;

namespace Write.TestTooling;

public record TransactionBuilder(Guid Id, Guid AccountId, string ExternalId, decimal Amount, string Label,
    DateTime Date, Guid? CategoryId)
{
    public static TransactionBuilder For(Guid id) =>
        new(id, Guid.Parse("B7675755-4E2D-49D3-9E7E-D1CF1EA6A84C"), "External id", 552.75m, "The label",
            DateTime.Parse("2023-04-19"), Guid.Parse("74F9CEF9-3F7A-4044-9E81-4F95307B58E1"));

    public Transaction Build()
    {
        return Transaction.From(new TransactionSnapshot(this.Id, this.AccountId, this.ExternalId, this.Amount,
            this.Label, this.Date, this.CategoryId));
    }
}
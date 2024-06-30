using Write.App.Model.Transactions;

namespace Write.TestTooling;

public record TransactionBuilder(
    Guid Id,
    Guid AccountId,
    string ExternalId,
    decimal Amount,
    string Label,
    DateTime Date,
    Guid? CategoryId,
    string? CategoryLabel)
{
    public Transaction Build() =>
        Transaction.From(this.ToSnapshot());

    public TransactionSnapshot ToSnapshot() =>
        new(this.Id, this.AccountId, this.ExternalId, this.Amount, this.Label, this.Date, this.CategoryId);
}

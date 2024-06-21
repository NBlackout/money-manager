using Shared.Presentation;
using Write.App.Model.Transactions;

namespace Read.TestTooling;

public record TransactionBuilder(
    Guid Id,
    Guid AccountId,
    decimal Amount,
    string Label,
    DateTime Date,
    CategoryBuilder? Category)
{
    public static TransactionBuilder Create()
    {
        return new TransactionBuilder(
            Guid.NewGuid(),
            Guid.Parse("D5611488-65FE-469E-BA46-32D4E0730C08"),
            1234.56m,
            "Label",
            DateTime.Parse("2021-06-01"),
            CategoryBuilder.Create()
        );
    }

    public Transaction Build()
    {
        return Transaction.From(
            new TransactionSnapshot(
                this.Id,
                this.AccountId,
                "External id",
                this.Amount,
                this.Label,
                this.Date,
                this.Category?.Id
            )
        );
    }

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date, this.Category?.Label);
}
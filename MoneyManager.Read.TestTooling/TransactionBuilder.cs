using MoneyManager.Shared.Presentation;
using MoneyManager.Write.Application.Model.Transactions;

namespace MoneyManager.Read.TestTooling;

public record TransactionBuilder(Guid Id, Guid AccountId, decimal Amount, string Label, DateTime Date,
    CategoryBuilder? Category)
{
    public static TransactionBuilder For(Guid id)
    {
        return new TransactionBuilder(id, Guid.Parse("D5611488-65FE-469E-BA46-32D4E0730C08"), 1234.56m, "Label",
            DateTime.Parse("2021-06-01"), CategoryBuilder.For(Guid.Parse("EDC81ABE-2E95-46B2-B008-BA41C3100A69"))
        );
    }

    public Transaction Build()
    {
        return Transaction.From(new TransactionSnapshot(this.Id, this.AccountId, "External id", this.Amount, this.Label,
            this.Date, this.Category?.Id)
        );
    }

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date, this.Category?.Label);
}
using MoneyManager.Shared.Presentation;
using MoneyManager.Write.Application.Model.Transactions;

namespace MoneyManager.Read.TestTooling;

public record TransactionBuilder(Guid Id, Guid AccountId, decimal Amount, string Label, DateTime Date)
{
    public static TransactionBuilder For(Guid id) =>
        new(id, Guid.Parse("D5611488-65FE-469E-BA46-32D4E0730C08"), 1234.56m, "Label", DateTime.Parse("2021-06-01"));

    public Transaction Build() =>
        Transaction.From(new TransactionSnapshot(this.Id, this.AccountId, "External id", this.Amount, this.Label,
            this.Date, Guid.Parse("EDC81ABE-2E95-46B2-B008-BA41C3100A69")));

    public TransactionSummaryPresentation ToSummary() =>
        new(this.Id, this.Amount, this.Label, this.Date);
}
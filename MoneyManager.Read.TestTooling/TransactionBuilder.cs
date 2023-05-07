using MoneyManager.Write.Application.Model.Transactions;

namespace MoneyManager.Read.TestTooling;

public record TransactionBuilder(Guid Id, Guid AccountId, decimal Amount)
{
    public static TransactionBuilder For(Guid id) =>
        new(id, Guid.Parse("D5611488-65FE-469E-BA46-32D4E0730C08"), 1234.56m);

    public Transaction Build() =>
        Transaction.From(new TransactionSnapshot(this.Id, this.AccountId, "External id", this.Amount));
}
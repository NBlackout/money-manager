using MoneyManager.Write.Application.Model.Banks;

namespace MoneyManager.Write.TestTooling;

public record BankBuilder(Guid Id, string ExternalId, string Name)
{
    public static BankBuilder For(Guid id) =>
        new(id, "0052158911", "The name");

    public Bank Build() =>
        Bank.From(new BankSnapshot(this.Id, this.ExternalId));
}
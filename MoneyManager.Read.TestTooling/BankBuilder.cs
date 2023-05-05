using MoneyManager.Write.Application.Model;

namespace MoneyManager.Read.TestTooling;

public record BankBuilder(Guid Id, string Name)
{
    public static BankBuilder For(Guid id) =>
        new(id, "A name");

    public Bank Build() =>
        Bank.From(new BankSnapshot(this.Id, "External id", this.Name));
}
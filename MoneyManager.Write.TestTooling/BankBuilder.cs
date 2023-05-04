namespace MoneyManager.Write.TestTooling;

public record BankBuilder(Guid Id, string ExternalId)
{
    public static BankBuilder For(Guid id) =>
        new(id, "0052158911");

    public Bank Build() => 
        Bank.From(new BankSnapshot(this.Id, this.ExternalId));
}
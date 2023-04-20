namespace MoneyManager.Application.Write.Model;

public record AccountId(string BankIdentifier, string Number)
{
    public Account Track() =>
        new(this);
}
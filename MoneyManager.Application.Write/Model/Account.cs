namespace MoneyManager.Application.Write.Model;

public class Account
{
    public AccountId Id { get; }

    public Account(AccountId id)
    {
        this.Id = id;
    }
}
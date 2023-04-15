namespace MoneyManager.Application.Write.Model;

public class Account
{
    public string Number { get; }

    public Account(string number)
    {
        this.Number = number;
    }
}
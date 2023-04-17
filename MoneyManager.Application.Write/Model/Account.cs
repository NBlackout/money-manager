namespace MoneyManager.Application.Write.Model;

public class Account
{
    public Guid Id { get; }
    public string Number { get; }

    public Account(Guid id, string number)
    {
        this.Id = id;
        this.Number = number;
    }
}
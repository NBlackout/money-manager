namespace MoneyManager.Application.Write.Model;

public record AccountIdentification(string Number)
{
    public Account Track(Guid id) =>
        new(id, this.Number);
}
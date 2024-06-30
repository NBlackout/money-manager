namespace Client.Write.App.Ports;

public interface IBudgetGateway
{
    Task Define(Guid id, string name, decimal amount);
}
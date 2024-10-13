namespace Client.Write.Infra.Gateways.Budget;

public class SpyBudgetGateway : IBudgetGateway
{
    public List<(Guid, string, decimal)> DefineCalls { get; } = [];

    public Task Define(Guid id, string name, decimal amount)
    {
        this.DefineCalls.Add((id, name, amount));

        return Task.CompletedTask;
    }
}
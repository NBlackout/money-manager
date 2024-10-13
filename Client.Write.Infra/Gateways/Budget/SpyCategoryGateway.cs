namespace Client.Write.Infra.Gateways.Budget;

public class SpyBudgetGateway : IBudgetGateway
{
    public List<(Guid, string, decimal, DateOnly)> DefineCalls { get; } = [];

    public Task Define(Guid id, string name, decimal amount, DateOnly beginDate)
    {
        this.DefineCalls.Add((id, name, amount, beginDate));

        return Task.CompletedTask;
    }
}
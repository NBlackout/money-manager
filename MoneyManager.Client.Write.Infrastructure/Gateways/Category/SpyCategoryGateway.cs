namespace MoneyManager.Client.Write.Infrastructure.Gateways.Category;

public class SpyCategoryGateway : ICategoryGateway
{
    public List<(Guid, string)> Calls { get; } = new();

    public Task Create(Guid id, string label)
    {
        this.Calls.Add((id, label));

        return Task.CompletedTask;
    }
}
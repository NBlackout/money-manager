namespace Client.Write.Infra.Gateways.Category;

public class SpyCategoryGateway : ICategoryGateway
{
    public List<(Guid, string, string)> Calls { get; } = new();

    public Task Create(Guid id, string label, string pattern)
    {
        this.Calls.Add((id, label, pattern));

        return Task.CompletedTask;
    }
}
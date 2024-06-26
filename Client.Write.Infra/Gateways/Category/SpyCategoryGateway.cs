namespace Client.Write.Infra.Gateways.Category;

public class SpyCategoryGateway : ICategoryGateway
{
    public List<(Guid, string, string)> CreateCalls { get; } = new();
    public List<Guid> DeleteCalls { get; } = new();

    public Task Create(Guid id, string label, string keywords)
    {
        this.CreateCalls.Add((id, label, keywords));

        return Task.CompletedTask;
    }

    public Task Delete(Guid id)
    {
        this.DeleteCalls.Add(id);

        return Task.CompletedTask;
    }
}
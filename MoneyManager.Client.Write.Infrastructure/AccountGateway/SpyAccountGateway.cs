namespace MoneyManager.Client.Write.Infrastructure.AccountGateway;

public class SpyAccountGateway : IAccountGateway
{
    public List<Guid> StopTrackingCalls { get; } = new();
    public List<Guid> ResumeTrackingCalls { get; } = new();
    public List<(Guid, string)> AssignLabelCalls { get; } = new();

    public Task StopTracking(Guid id)
    {
        this.StopTrackingCalls.Add(id);

        return Task.CompletedTask;
    }

    public Task ResumeTracking(Guid id)
    {
        this.ResumeTrackingCalls.Add(id);

        return Task.CompletedTask;
    }

    public Task AssignLabel(Guid id, string label)
    {
        this.AssignLabelCalls.Add((id, label));

        return Task.CompletedTask;
    }
}
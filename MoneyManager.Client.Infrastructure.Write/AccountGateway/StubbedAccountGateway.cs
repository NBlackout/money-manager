namespace MoneyManager.Client.Infrastructure.Write.AccountGateway;

public class StubbedAccountGateway : IAccountGateway
{
    public List<Guid> Calls { get; } = new();

    public Task StopTracking(Guid id)
    {
        this.Calls.Add(id);

        return Task.CompletedTask;
    }
}
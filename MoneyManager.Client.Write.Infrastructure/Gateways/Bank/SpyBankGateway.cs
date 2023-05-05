namespace MoneyManager.Client.Write.Infrastructure.Gateways.Bank;

public class SpyBankGateway : IBankGateway
{
    public List<(Guid, string)> AssignNameCalls { get; set; } = new();

    public Task AssignName(Guid id, string name)
    {
        this.AssignNameCalls.Add((id, name));

        return Task.CompletedTask;
    }
}
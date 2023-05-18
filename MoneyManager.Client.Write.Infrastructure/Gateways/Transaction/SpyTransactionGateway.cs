namespace MoneyManager.Client.Write.Infrastructure.Gateways.Transaction;

public class SpyTransactionGateway : ITransactionGateway
{
    public List<(Guid, Guid)> AssignCategoryCalls { get; } = new();

    public Task AssignCategory(Guid transactionId, Guid categoryId)
    {
        this.AssignCategoryCalls.Add((transactionId, categoryId));

        return Task.CompletedTask;
    }
}
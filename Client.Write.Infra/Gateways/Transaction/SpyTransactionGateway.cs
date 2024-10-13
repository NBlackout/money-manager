namespace Client.Write.Infra.Gateways.Transaction;

public class SpyTransactionGateway : ITransactionGateway
{
    public List<(Guid, Guid)> AssignCategoryCalls { get; } = [];

    public Task AssignCategory(Guid transactionId, Guid categoryId)
    {
        this.AssignCategoryCalls.Add((transactionId, categoryId));

        return Task.CompletedTask;
    }
}
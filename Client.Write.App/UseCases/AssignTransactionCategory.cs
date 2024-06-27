namespace Client.Write.App.UseCases;

public class AssignTransactionCategory(ITransactionGateway gateway)
{
    public async Task Execute(Guid transactionId, Guid categoryId) =>
        await gateway.AssignCategory(transactionId, categoryId);
}
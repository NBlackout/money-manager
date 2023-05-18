namespace MoneyManager.Client.Write.Application.UseCases;

public class AssignTransactionCategory
{
    private readonly ITransactionGateway gateway;

    public AssignTransactionCategory(ITransactionGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(Guid transactionId, Guid categoryId) =>
        await this.gateway.AssignCategory(transactionId, categoryId);
}
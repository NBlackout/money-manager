namespace MoneyManager.Client.Write.Application.Ports;

public interface ITransactionGateway
{
    Task AssignCategory(Guid transactionId, Guid categoryId);
}
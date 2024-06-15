namespace Client.Write.App.Ports;

public interface ITransactionGateway
{
    Task AssignCategory(Guid transactionId, Guid categoryId);
}
namespace MoneyManager.Client.Read.Application.Ports;

public interface ITransactionsOfMonthGateway
{
    Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid accountId, int year, int month);
}
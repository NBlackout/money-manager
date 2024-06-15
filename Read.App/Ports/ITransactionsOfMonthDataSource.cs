namespace Read.App.Ports;

public interface ITransactionsOfMonthDataSource
{
    Task<TransactionSummaryPresentation[]> Get(Guid accountId, int year, int month);
}
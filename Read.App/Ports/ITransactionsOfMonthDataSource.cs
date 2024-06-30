namespace Read.App.Ports;

public interface ITransactionsOfMonthDataSource
{
    Task<TransactionSummaryPresentation[]> By(Guid accountId, int year, int month);
}
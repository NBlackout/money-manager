namespace Read.App.Ports;

public interface ITransactionsOfMonthDataSource
{
    Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid accountId, int year, int month);
}
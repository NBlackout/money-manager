namespace Read.App.UseCases;

public class TransactionsOfMonth(ITransactionsOfMonthDataSource dataSource)
{
    public async Task<TransactionSummaryPresentation[]> Execute(Guid accountId, int year, int month) =>
        await dataSource.Get(accountId, year, month);
}

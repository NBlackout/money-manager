using App.Read.Ports;

namespace App.Read.UseCases;

public class TransactionsOfMonth(ITransactionsOfMonthDataSource dataSource)
{
    public async Task<TransactionSummaryPresentation[]> Execute(Guid accountId, int year, int month) =>
        await dataSource.By(accountId, year, month);
}
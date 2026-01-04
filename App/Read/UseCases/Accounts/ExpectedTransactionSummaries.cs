using App.Read.Ports;

namespace App.Read.UseCases.Accounts;

public class ExpectedTransactionSummaries(IExpectedTransactionSummariesDataSource dataSource)
{
    public async Task<ExpectedTransactionSummaryPresentation[]> Execute(int year, int month) =>
        await dataSource.By(year, month);
}
using App.Read.Ports;

namespace App.Read.UseCases;

public class ExpectedTransactionSummaries(IExpectedTransactionSummariesDataSource dataSource)
{
    public async Task<ExpectedTransactionSummaryPresentation[]> Execute() =>
        await dataSource.All();
}
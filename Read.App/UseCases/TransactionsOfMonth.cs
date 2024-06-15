namespace Read.App.UseCases;

public class TransactionsOfMonth
{
    private readonly ITransactionsOfMonthDataSource dataSource;

    public TransactionsOfMonth(ITransactionsOfMonthDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<TransactionSummaryPresentation[]> Execute(Guid accountId, int year, int month) =>
        await this.dataSource.Get(accountId, year, month);
}
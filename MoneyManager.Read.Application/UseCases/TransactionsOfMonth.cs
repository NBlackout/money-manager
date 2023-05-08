namespace MoneyManager.Read.Application.UseCases;

public class TransactionsOfMonth
{
    private readonly ITransactionsOfMonthDataSource dataSource;

    public TransactionsOfMonth(ITransactionsOfMonthDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<IReadOnlyCollection<TransactionSummaryPresentation>> Execute(Guid accountId, int year, int month) =>
        await this.dataSource.Get(accountId, year, month);
}
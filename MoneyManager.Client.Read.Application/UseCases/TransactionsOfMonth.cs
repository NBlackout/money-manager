namespace MoneyManager.Client.Read.Application.UseCases;

public class TransactionsOfMonth
{
    private readonly ITransactionsOfMonthGateway gateway;

    public TransactionsOfMonth(ITransactionsOfMonthGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<IReadOnlyCollection<TransactionSummaryPresentation>> Execute(Guid id, int year, int month) =>
        await this.gateway.Get(id, year, month);
}
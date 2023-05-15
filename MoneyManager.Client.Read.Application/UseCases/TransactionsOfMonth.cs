namespace MoneyManager.Client.Read.Application.UseCases;

public class TransactionsOfMonth
{
    private readonly IAccountGateway gateway;

    public TransactionsOfMonth(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<IReadOnlyCollection<TransactionSummaryPresentation>> Execute(Guid id, int year, int month) =>
        await this.gateway.TransactionsOfMonth(id, year, month);
}
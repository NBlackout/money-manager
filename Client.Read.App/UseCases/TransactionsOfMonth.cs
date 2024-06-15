namespace Client.Read.App.UseCases;

public class TransactionsOfMonth
{
    private readonly IAccountGateway gateway;

    public TransactionsOfMonth(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<TransactionSummaryPresentation[]> Execute(Guid id, int year, int month) =>
        await this.gateway.TransactionsOfMonth(id, year, month);
}
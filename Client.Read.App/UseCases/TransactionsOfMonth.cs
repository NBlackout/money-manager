namespace Client.Read.App.UseCases;

public class TransactionsOfMonth(IAccountGateway gateway)
{
    public async Task<TransactionSummaryPresentation[]> Execute(Guid id, int year, int month) =>
        await gateway.TransactionsOfMonth(id, year, month);
}
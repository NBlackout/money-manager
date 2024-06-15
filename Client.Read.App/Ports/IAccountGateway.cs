namespace Client.Read.App.Ports;

public interface IAccountGateway
{
    Task<IReadOnlyCollection<AccountSummaryPresentation>> Summaries();
    Task<AccountDetailsPresentation> Details(Guid id);
    Task<IReadOnlyCollection<TransactionSummaryPresentation>> TransactionsOfMonth(Guid accountId, int year, int month);
}
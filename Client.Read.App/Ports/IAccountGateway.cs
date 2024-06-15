namespace Client.Read.App.Ports;

public interface IAccountGateway
{
    Task<AccountSummaryPresentation[]> Summaries();
    Task<AccountDetailsPresentation> Details(Guid id);
    Task<TransactionSummaryPresentation[]> TransactionsOfMonth(Guid accountId, int year, int month);
}
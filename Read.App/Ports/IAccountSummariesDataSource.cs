namespace Read.App.Ports;

public interface IAccountSummariesDataSource
{
    Task<AccountSummaryPresentation[]> Get();
}
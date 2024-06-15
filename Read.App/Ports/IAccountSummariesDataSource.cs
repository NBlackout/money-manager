namespace Read.App.Ports;

public interface IAccountSummariesDataSource
{
    Task<IReadOnlyCollection<AccountSummaryPresentation>> Get();
}
namespace MoneyManager.Read.Application.Ports;

public interface IAccountSummariesDataSource
{
    Task<IReadOnlyCollection<AccountSummaryPresentation>> Get();
}
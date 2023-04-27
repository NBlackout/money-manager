namespace MoneyManager.Read.Application.Ports;

public interface IAccountSummariesDataSource
{
    Task<IReadOnlyCollection<AccountSummary>> Get();
}
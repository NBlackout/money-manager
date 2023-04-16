namespace MoneyManager.Application.Read.Ports;

public interface IAccountSummariesDataSource
{
    Task<IReadOnlyCollection<AccountSummary>> Get();
}
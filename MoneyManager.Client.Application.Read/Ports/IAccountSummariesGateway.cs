namespace MoneyManager.Client.Application.Read.Ports;

public interface IAccountSummariesGateway
{
    Task<IReadOnlyCollection<AccountSummary>> Get();
}
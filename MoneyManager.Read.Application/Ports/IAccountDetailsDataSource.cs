namespace MoneyManager.Read.Application.Ports;

public interface IAccountDetailsDataSource
{
    Task<AccountDetailsPresentation> Get(Guid id);
}
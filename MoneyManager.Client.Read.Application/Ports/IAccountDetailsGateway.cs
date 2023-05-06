namespace MoneyManager.Client.Read.Application.Ports;

public interface IAccountDetailsGateway
{
    Task<AccountDetailsPresentation> Get(Guid id);
}
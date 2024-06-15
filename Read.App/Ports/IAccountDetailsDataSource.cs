namespace Read.App.Ports;

public interface IAccountDetailsDataSource
{
    Task<AccountDetailsPresentation> Get(Guid id);
}
namespace Read.App.Ports;

public interface IAccountDetailsDataSource
{
    Task<AccountDetailsPresentation> By(Guid id);
}

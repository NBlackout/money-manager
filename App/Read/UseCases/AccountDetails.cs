using App.Read.Ports;

namespace App.Read.UseCases;

public class AccountDetails(IAccountDetailsDataSource dataSource)
{
    public async Task<AccountDetailsPresentation> Execute(Guid id) =>
        await dataSource.By(id);
}
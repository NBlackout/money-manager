using App.Read.Ports;

namespace App.Read.UseCases.Accounts;

public class AccountDetails(IAccountDetailsDataSource dataSource)
{
    public async Task<AccountDetailsPresentation> Execute(Guid id) =>
        await dataSource.By(id);
}
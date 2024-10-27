namespace Read.App.UseCases;

public class AccountDetails(IAccountDetailsDataSource dataSource)
{
    public async Task<AccountDetailsPresentation> Execute(Guid id) =>
        await dataSource.By(id);
}
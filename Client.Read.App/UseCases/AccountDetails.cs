namespace Client.Read.App.UseCases;

public class AccountDetails(IAccountGateway gateway)
{
    public async Task<AccountDetailsPresentation> Execute(Guid id) =>
        await gateway.Details(id);
}
namespace Client.Read.App.UseCases;

public class AccountDetails
{
    private readonly IAccountGateway gateway;

    public AccountDetails(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<AccountDetailsPresentation> Execute(Guid id) => 
        await this.gateway.Details(id);
}
namespace MoneyManager.Client.Read.Application.UseCases;

public class AccountDetails
{
    private readonly IAccountDetailsGateway gateway;

    public AccountDetails(IAccountDetailsGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<AccountDetailsPresentation> Execute(Guid id) =>
        await this.gateway.Get(id);
}
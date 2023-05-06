namespace MoneyManager.Client.Read.Application.UseCases;

public class AccountSummaries
{
    private readonly IAccountSummariesGateway gateway;

    public AccountSummaries(IAccountSummariesGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Execute() =>
        await this.gateway.Get();
}
namespace MoneyManager.Client.Read.Application.UseCases;

public class AccountSummaries
{
    private readonly IAccountGateway gateway;

    public AccountSummaries(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Execute() =>
        await this.gateway.Summaries();
}
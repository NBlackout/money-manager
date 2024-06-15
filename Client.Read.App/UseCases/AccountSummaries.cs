namespace Client.Read.App.UseCases;

public class AccountSummaries
{
    private readonly IAccountGateway gateway;

    public AccountSummaries(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<AccountSummaryPresentation[]> Execute() =>
        await this.gateway.Summaries();
}
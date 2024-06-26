namespace Client.Read.App.UseCases;

public class AccountSummaries(IAccountGateway gateway)
{
    public async Task<AccountSummaryPresentation[]> Execute() =>
        await gateway.Summaries();
}
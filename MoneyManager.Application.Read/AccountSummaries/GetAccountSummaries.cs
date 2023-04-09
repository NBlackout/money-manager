using MoneyManager.Application.Read.Ports;

namespace MoneyManager.Application.Read.AccountSummaries;

public class GetAccountSummaries
{
    private readonly IAccountSummariesDataSource dataSource;

    public GetAccountSummaries(IAccountSummariesDataSource dataSource) =>
        this.dataSource = dataSource;

    public async Task<IReadOnlyCollection<AccountSummary>> Handle() =>
        await this.dataSource.Get();
}
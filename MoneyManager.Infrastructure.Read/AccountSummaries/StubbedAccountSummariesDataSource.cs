using MoneyManager.Application.Read.AccountSummaries;
using MoneyManager.Application.Read.Ports;

namespace MoneyManager.Infrastructure.Read.AccountSummaries;

public class StubbedAccountSummariesDataSource : IAccountSummariesDataSource
{
    private readonly IReadOnlyCollection<AccountSummary> data;

    public StubbedAccountSummariesDataSource(IReadOnlyCollection<AccountSummary> data) =>
        this.data = data;

    public Task<IReadOnlyCollection<AccountSummary>> Get() =>
        Task.FromResult(data);
}
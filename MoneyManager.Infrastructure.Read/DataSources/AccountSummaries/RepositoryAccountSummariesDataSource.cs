using MoneyManager.Application.Write.Model;
using MoneyManager.Infrastructure.Write.Repositories;

namespace MoneyManager.Infrastructure.Read.DataSources.AccountSummaries;

public class RepositoryAccountSummariesDataSource : IAccountSummariesDataSource
{
    private readonly InMemoryAccountRepository repository;

    public RepositoryAccountSummariesDataSource(InMemoryAccountRepository repository)
    {
        this.repository = repository;
    }

    public Task<IReadOnlyCollection<AccountSummary>> Get()
    {
        IReadOnlyCollection<AccountSummary> summaries = this.repository.Data.Select(ToSummary).ToList();

        return Task.FromResult(summaries);
    }

    private static AccountSummary ToSummary(AccountSnapshot account) =>
        new(account.Id, account.Number, account.Balance);
}
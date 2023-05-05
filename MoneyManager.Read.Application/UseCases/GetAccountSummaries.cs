﻿namespace MoneyManager.Read.Application.UseCases;

public class GetAccountSummaries
{
    private readonly IAccountSummariesDataSource dataSource;

    public GetAccountSummaries(IAccountSummariesDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Execute() =>
        await this.dataSource.Get();
}
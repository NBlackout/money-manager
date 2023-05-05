﻿namespace MoneyManager.Client.Read.Application.UseCases.AccountSummaries;

public class GetAccountSummaries
{
    private readonly IAccountSummariesGateway gateway;

    public GetAccountSummaries(IAccountSummariesGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<IReadOnlyCollection<AccountSummary>> Execute() =>
        await this.gateway.Get();
}
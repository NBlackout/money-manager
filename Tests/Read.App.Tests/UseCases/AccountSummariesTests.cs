﻿namespace Read.App.Tests.UseCases;

public class AccountSummariesTests
{
    private readonly StubbedAccountSummariesDataSource dataSource = new();
    private readonly AccountSummaries sut;

    public AccountSummariesTests()
    {
        this.sut = new AccountSummaries(this.dataSource);
    }

    [Theory]
    [RandomData]
    public async Task Gives_account_summaries(AccountSummaryPresentation[] expected)
    {
        this.dataSource.Feed(expected);
        AccountSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}
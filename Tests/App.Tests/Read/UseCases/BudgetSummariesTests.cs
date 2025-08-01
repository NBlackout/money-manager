﻿using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class BudgetSummariesTests
{
    private readonly StubbedBudgetSummariesDataSource dataSource = new();
    private readonly BudgetSummaries sut;

    public BudgetSummariesTests()
    {
        this.sut = new BudgetSummaries(this.dataSource);
    }

    [Theory]
    [RandomData]
    public async Task Gives_budget_summaries(BudgetSummaryPresentation[] expected)
    {
        this.dataSource.Feed(expected);
        BudgetSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}
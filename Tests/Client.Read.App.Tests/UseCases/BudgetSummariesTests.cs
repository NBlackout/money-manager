﻿using Client.Read.Infra.Gateways.Budget;

namespace Client.Read.App.Tests.UseCases;

public class BudgetSummariesTests
{
    private readonly StubbedBudgetGateway gateway = new();
    private readonly BudgetSummaries sut;

    public BudgetSummariesTests()
    {
        this.sut = new BudgetSummaries(this.gateway);
    }

    [Theory, RandomData]
    public async Task Retrieves_budget_summaries(BudgetSummaryPresentation[] expected)
    {
        this.gateway.Feed(expected);
        BudgetSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}
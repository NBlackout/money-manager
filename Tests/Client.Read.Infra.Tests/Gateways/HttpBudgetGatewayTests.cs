﻿using Client.Read.Infra.Gateways.Budget;
using Client.Read.Infra.Tests.TestDoubles;
using Shared.Presentation;

namespace Client.Read.Infra.Tests.Gateways;

public sealed class HttpBudgetGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();
    private readonly HttpBudgetGateway sut;

    public HttpBudgetGatewayTests()
    {
        this.sut = this.Resolve<IBudgetGateway, HttpBudgetGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddReadDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Retrieves_budget_summaries(BudgetSummaryPresentation[] expected)
    {
        this.httpMessageHandler.Feed($"{ApiUrl}/budgets", expected);
        BudgetSummaryPresentation[] actual = await this.sut.Summaries();
        actual.Should().Equal(expected);
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}
﻿using System.Text.Json;
using Client.Write.Infra.Gateways.Budget;
using Client.Write.Infra.Tests.TestDoubles;
using Shared.Infra.TestTooling;

namespace Client.Write.Infra.Tests.Gateways;

public sealed class HttpBudgetGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();
    private readonly HttpBudgetGateway sut;

    public HttpBudgetGatewayTests()
    {
        this.sut = this.Resolve<IBudgetGateway, HttpBudgetGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Defines(Guid id, string name, decimal amount, DateOnly beginDate)
    {
        await this.sut.Define(id, name, amount, beginDate);
        this.Verify_Post($"{ApiUrl}/budgets", new { Id = id, Name = name, Amount = amount, BeginDate = beginDate });
    }

    private void Verify_Post(string url, object payload)
    {
        this.httpMessageHandler.Calls.Should()
            .Equal((HttpMethod.Post, url, JsonSerializer.Serialize(payload, Defaults.JsonSerializerOptions)));
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}
using System.Net.Http.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Infrastructure.Write.AccountGateway;
using MoneyManager.Shared;

namespace MoneyManager.Client.Infrastructure.Tests;

public sealed class HttpAccountGatewayTests : IDisposable
{
    private readonly HttpClient httpClient;
    private readonly IHost host;
    private readonly HttpAccountGateway sut;

    public HttpAccountGatewayTests()
    {
        this.httpClient = CreateApiClient();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddScoped(_ => this.httpClient))
            .Build();
        this.sut = this.host.GetRequiredService<IAccountGateway, HttpAccountGateway>();
    }

    [Fact(Skip = "Temporary")]
    public async Task Should_stop_tracking_account()
    {
        Guid id = Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C");

        await this.sut.StopTracking(id);

        IReadOnlyCollection<AccountSummary> accounts =
            (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummary>>(""))!;
        accounts.Single(a => a.Id == id).Tracked.Should().BeFalse();

        await this.httpClient.PutAsJsonAsync("", new UpdateTrackedStatus(true));
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateApiClient()
    {
        Uri apiUri = new("https://nblackout-money-manager.azurewebsites.net/api/accounts/");

        return new HttpClient { BaseAddress = apiUri };
    }

    private sealed record UpdateTrackedStatus(bool Tracked);
}
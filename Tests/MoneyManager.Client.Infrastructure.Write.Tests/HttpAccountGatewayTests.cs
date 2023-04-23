using System.Net.Http.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Infrastructure.Write.AccountGateway;
using MoneyManager.Shared;

namespace MoneyManager.Client.Infrastructure.Write.Tests;

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

    [Fact]
    public async Task Should_stop_tracking_account()
    {
        Guid id = Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C");

        await this.sut.StopTracking(id);

        IReadOnlyCollection<AccountSummary> accounts =
            (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummary>>("accounts"))!;
        accounts.Single(a => a.Id == id).Tracked.Should().BeFalse();

        (await this.httpClient.PutAsJsonAsync($"accounts/{id}/tracking", new ChangeTrackingStatusDto(true)))
            .EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Should_resume_account_tracking()
    {
        Guid id = Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A");

        await this.sut.ResumeTracking(id);

        IReadOnlyCollection<AccountSummary> accounts =
            (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummary>>("accounts"))!;
        accounts.Single(a => a.Id == id).Tracked.Should().BeTrue();

        (await this.httpClient.PutAsJsonAsync($"accounts/{id}/tracking", new ChangeTrackingStatusDto(false)))
            .EnsureSuccessStatusCode();
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateApiClient()
    {
        Uri apiUri = new("https://nblackout-money-manager.azurewebsites.net/api/");

        return new HttpClient { BaseAddress = apiUri };
    }
}
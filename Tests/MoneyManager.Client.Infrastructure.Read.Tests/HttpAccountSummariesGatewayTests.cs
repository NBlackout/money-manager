using MoneyManager.Client.Infrastructure.Read.AccountSummariesGateway;

namespace MoneyManager.Client.Infrastructure.Read.Tests;

public sealed class HttpAccountSummariesGatewayTests : IDisposable
{
    private readonly IHost host;
    private readonly HttpAccountSummariesGateway sut;

    public HttpAccountSummariesGatewayTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddReadDependencies().AddScoped(CreateApi))
            .Build();
        this.sut = this.host.GetRequiredService<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }

    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        IReadOnlyCollection<AccountSummary> actual = await this.sut.Get();

        AccountSummary[] expected =
        {
            new(Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C"), "Compte joint", 12345.67m),
            new(Guid.Parse("603F21F4-CE85-42AB-9E7E-87C9CFFE0F6D"), "Livret", 89.00m),
            new(Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A"), "Epargne", 1000.00m)
        };
        actual.Should().Equal(expected);
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateApi(IServiceProvider provider)
    {
        Uri apiUri = new("https://nblackout-money-manager.azurewebsites.net/api/");

        return new HttpClient { BaseAddress = apiUri };
    }
}
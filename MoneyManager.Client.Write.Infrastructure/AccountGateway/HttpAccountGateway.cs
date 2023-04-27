using System.Net.Http.Json;
using MoneyManager.Shared;

namespace MoneyManager.Client.Write.Infrastructure.AccountGateway;

public class HttpAccountGateway : IAccountGateway
{
    private readonly HttpClient httpClient;

    public HttpAccountGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task StopTracking(Guid id) =>
        await this.ChangeTrackingStatus(id, false);

    public async Task ResumeTracking(Guid id) =>
        await this.ChangeTrackingStatus(id, true);

    private async Task ChangeTrackingStatus(Guid id, bool enabled)
    {
        (await this.httpClient.PutAsJsonAsync($"accounts/{id}/tracking", new ChangeTrackingStatusDto(enabled)))
            .EnsureSuccessStatusCode();
    }
}
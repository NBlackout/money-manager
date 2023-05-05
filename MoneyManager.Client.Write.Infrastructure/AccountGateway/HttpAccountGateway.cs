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

    public async Task AssignLabel(Guid id, string label)
    {
        (await this.httpClient.PutAsJsonAsync($"accounts/{id}/label", new AssignAccountLabelDto(label)))
            .EnsureSuccessStatusCode();
    }

    private async Task ChangeTrackingStatus(Guid id, bool enabled)
    {
        (await this.httpClient.PutAsJsonAsync($"accounts/{id}/tracking", new ChangeTrackingStatusDto(enabled)))
            .EnsureSuccessStatusCode();
    }
}
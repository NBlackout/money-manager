namespace Client.Write.Infra.Gateways;

public class HttpAccountGateway(HttpClient httpClient) : IAccountGateway
{
    public async Task StopTracking(Guid id) =>
        await this.ChangeTrackingStatus(id, false);

    public async Task ResumeTracking(Guid id) =>
        await this.ChangeTrackingStatus(id, true);

    public async Task AssignLabel(Guid id, string label)
    {
        (await httpClient.PutAsJsonAsync($"accounts/{id}/label", new AccountLabelDto(label)))
            .EnsureSuccessStatusCode();
    }

    private async Task ChangeTrackingStatus(Guid id, bool enabled)
    {
        (await httpClient.PutAsJsonAsync($"accounts/{id}/tracking", new TrackingStatusDto(enabled)))
            .EnsureSuccessStatusCode();
    }
}

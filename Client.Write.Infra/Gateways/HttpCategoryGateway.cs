namespace Client.Write.Infra.Gateways;

public class HttpCategoryGateway(HttpClient httpClient) : ICategoryGateway
{
    public async Task Create(Guid id, string label, string keywords) =>
        (await httpClient.PostAsJsonAsync("api/categories", new CategoryDto(id, label, keywords))).EnsureSuccessStatusCode();

    public async Task Delete(Guid id) =>
        (await httpClient.DeleteAsync($"api/categories/{id}")).EnsureSuccessStatusCode();
}
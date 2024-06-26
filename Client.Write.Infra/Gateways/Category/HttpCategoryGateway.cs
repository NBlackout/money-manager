﻿namespace Client.Write.Infra.Gateways.Category;

public class HttpCategoryGateway : ICategoryGateway
{
    private readonly HttpClient httpClient;

    public HttpCategoryGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Create(Guid id, string label, string keywords) =>
        (await this.httpClient.PostAsJsonAsync("categories", new CategoryDto(id, label, keywords))).EnsureSuccessStatusCode();

    public async Task Delete(Guid id) =>
        (await this.httpClient.DeleteAsync($"categories/{id}")).EnsureSuccessStatusCode();
}
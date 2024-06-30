﻿namespace Client.Write.Infra.Gateways.Budget;

public class HttpBudgetGateway(HttpClient httpClient) : IBudgetGateway
{
    public async Task Define(Guid id, string name, decimal amount) =>
        (await httpClient.PostAsJsonAsync("budgets", new BudgetDto(id, name, amount))).EnsureSuccessStatusCode();
}
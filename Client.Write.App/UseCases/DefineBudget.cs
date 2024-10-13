namespace Client.Write.App.UseCases;

public class DefineBudget(IBudgetGateway gateway)
{
    public async Task Execute(Guid id, string name, decimal amount, DateOnly beginDate) =>
        await gateway.Define(id, name, amount, beginDate);
}
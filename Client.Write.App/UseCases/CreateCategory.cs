namespace Client.Write.App.UseCases;

public class CreateCategory(ICategoryGateway gateway)
{
    public async Task Execute(Guid id, string label, string keywords) =>
        await gateway.Create(id, label, keywords);
}
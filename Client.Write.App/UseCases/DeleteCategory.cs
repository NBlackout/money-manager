namespace Client.Write.App.UseCases;

public class DeleteCategory(ICategoryGateway gateway)
{
    public async Task Execute(Guid id) =>
        await gateway.Delete(id);
}
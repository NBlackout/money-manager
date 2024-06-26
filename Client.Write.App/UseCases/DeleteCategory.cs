namespace Client.Write.App.UseCases;

public class DeleteCategory
{
    private readonly ICategoryGateway gateway;

    public DeleteCategory(ICategoryGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(Guid id) => 
        await this.gateway.Delete(id);
}
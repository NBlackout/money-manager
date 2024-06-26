namespace Client.Write.App.UseCases;

public class CreateCategory
{
    private readonly ICategoryGateway gateway;

    public CreateCategory(ICategoryGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(Guid id, string label, string keywords) =>
        await this.gateway.Create(id, label, keywords);
}
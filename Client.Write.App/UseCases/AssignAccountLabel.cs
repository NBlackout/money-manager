namespace Client.Write.App.UseCases;

public class AssignAccountLabel(IAccountGateway gateway)
{
    public async Task Execute(Guid id, string label) =>
        await gateway.AssignLabel(id, label);
}
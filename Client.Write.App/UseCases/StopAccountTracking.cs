namespace Client.Write.App.UseCases;

public class StopAccountTracking(IAccountGateway gateway)
{
    public async Task Execute(Guid id) =>
        await gateway.StopTracking(id);
}
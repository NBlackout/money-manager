namespace Client.Write.App.UseCases;

public class StopAccountTracking(IAccountGateway accountGateway)
{
    public async Task Execute(Guid id) =>
        await accountGateway.StopTracking(id);
}

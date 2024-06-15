namespace Client.Write.App.UseCases;

public class StopAccountTracking
{
    private readonly IAccountGateway accountGateway;

    public StopAccountTracking(IAccountGateway accountGateway)
    {
        this.accountGateway = accountGateway;
    }

    public async Task Execute(Guid id) =>
        await this.accountGateway.StopTracking(id);
}
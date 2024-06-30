namespace Client.Write.App.UseCases;

public class ResumeAccountTracking(IAccountGateway gateway)
{
    public async Task Execute(Guid id) =>
        await gateway.ResumeTracking(id);
}
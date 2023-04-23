using MoneyManager.Client.Application.Write.Ports;

namespace MoneyManager.Client.Application.Write.UseCases;

public class ResumeAccountTracking
{
    private readonly IAccountGateway gateway;

    public ResumeAccountTracking(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(Guid id) =>
        await this.gateway.ResumeTracking(id);
}
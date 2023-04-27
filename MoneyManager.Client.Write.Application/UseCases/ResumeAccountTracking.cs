using MoneyManager.Client.Write.Application.Ports;

namespace MoneyManager.Client.Write.Application.UseCases;

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
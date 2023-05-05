using MoneyManager.Client.Write.Application.Ports;

namespace MoneyManager.Client.Write.Application.UseCases;

public class AssignBankName
{
    private readonly IBankGateway gateway;

    public AssignBankName(IBankGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(Guid id, string name) =>
        await this.gateway.AssignName(id, name);
}
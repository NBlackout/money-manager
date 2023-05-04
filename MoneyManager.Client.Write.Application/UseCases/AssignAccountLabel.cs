using MoneyManager.Client.Write.Application.Ports;

namespace MoneyManager.Client.Write.Application.UseCases;

public class AssignAccountLabel
{
    private readonly IAccountGateway gateway;

    public AssignAccountLabel(IAccountGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task Execute(Guid id, string label) =>
        await this.gateway.AssignLabel(id, label);
}
using Client.Write.Infra.Gateways.Account;

namespace Client.Write.App.Tests.UseCases;

public class AssignAccountLabelTests
{
    private readonly SpyAccountGateway gateway = new();
    private readonly AssignAccountLabel sut;

    public AssignAccountLabelTests()
    {
        this.sut = new AssignAccountLabel(this.gateway);
    }

    [Theory, RandomData]
    public async Task Should_assign_account_label(Guid id, string label)
    {
        await this.sut.Execute(id, label);
        this.gateway.AssignLabelCalls.Should().Equal((id, label));
    }
}
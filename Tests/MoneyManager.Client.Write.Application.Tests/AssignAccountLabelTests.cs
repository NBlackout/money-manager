using MoneyManager.Client.Write.Infrastructure.AccountGateway;

namespace MoneyManager.Client.Write.Application.Tests;

public class AssignAccountLabelTests
{
    private readonly SpyAccountGateway gateway;
    private readonly AssignAccountLabel sut;

    public AssignAccountLabelTests()
    {
        this.gateway = new SpyAccountGateway();
        this.sut = new AssignAccountLabel(this.gateway);
    }

    [Fact]
    public async Task Should_assign_account_label()
    {
        Guid id = Guid.NewGuid();
        const string label = "New label";

        await this.sut.Execute(id, label);

        this.gateway.AssignLabelCalls.Should().Equal((id, label));
    }
}
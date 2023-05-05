using MoneyManager.Client.Write.Infrastructure.Gateways.Bank;

namespace MoneyManager.Client.Write.Application.Tests.UseCases;

public class AssignBankNameTests
{
    private readonly SpyBankGateway gateway;
    private readonly AssignBankName sut;

    public AssignBankNameTests()
    {
        this.gateway = new SpyBankGateway();
        this.sut = new AssignBankName(this.gateway);
    }

    [Fact]
    public async Task Should_test_name()
    {
        Guid id = Guid.NewGuid();
        const string name = "New name";

        await this.sut.Execute(id, name);

        this.gateway.AssignNameCalls.Should().Equal((id, name));
    }
}
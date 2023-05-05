using MoneyManager.Client.Write.Infrastructure.Gateways.Account;

namespace MoneyManager.Client.Write.Application.Tests.UseCases;

public class StopAccountTrackingTests
{
    private readonly SpyAccountGateway accountGateway;
    private readonly StopAccountTracking sut;

    public StopAccountTrackingTests()
    {
        this.accountGateway = new SpyAccountGateway();
        this.sut = new StopAccountTracking(this.accountGateway);
    }

    [Fact]
    public async Task Should_stop_account_tracking()
    {
        Guid id = Guid.NewGuid();

        await this.sut.Execute(id);

        this.accountGateway.StopTrackingCalls.Should().Equal(id);
    }
}
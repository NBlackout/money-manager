using Client.Write.Infra.Gateways.Account;

namespace Client.Write.App.Tests.UseCases;

public class StopAccountTrackingTests
{
    private readonly SpyAccountGateway accountGateway = new();
    private readonly StopAccountTracking sut;

    public StopAccountTrackingTests()
    {
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
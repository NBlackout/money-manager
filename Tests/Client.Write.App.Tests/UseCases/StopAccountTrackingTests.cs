using Client.Write.App.Tests.TestDoubles;

namespace Client.Write.App.Tests.UseCases;

public class StopAccountTrackingTests
{
    private readonly SpyAccountGateway accountGateway = new();
    private readonly StopAccountTracking sut;

    public StopAccountTrackingTests()
    {
        this.sut = new StopAccountTracking(this.accountGateway);
    }

    [Theory]
    [RandomData]
    public async Task Stops_account_tracking(Guid id)
    {
        await this.sut.Execute(id);
        this.accountGateway.StopTrackingCalls.Should().Equal(id);
    }
}
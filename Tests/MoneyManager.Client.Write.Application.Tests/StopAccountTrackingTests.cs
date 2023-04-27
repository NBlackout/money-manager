using MoneyManager.Client.Write.Application.UseCases;
using MoneyManager.Client.Write.Infrastructure.AccountGateway;

namespace MoneyManager.Client.Write.Application.Tests;

public class StopAccountTrackingTests
{
    [Fact]
    public async Task Should_stop_account_tracking()
    {
        StubbedAccountGateway accountGateway = new();
        StopAccountTracking sut = new(accountGateway);

        Guid id = Guid.NewGuid();
        await sut.Execute(id);

        accountGateway.StopTrackingCalls.Should().Equal(id);
    }
}
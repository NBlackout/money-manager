using MoneyManager.Client.Application.Write.UseCases;
using MoneyManager.Client.Infrastructure.Write.AccountGateway;

namespace MoneyManager.Client.Application.Write.Tests;

public class StopAccountTrackingTests
{
    [Fact]
    public async Task Should_stop_account_tracking()
    {
        StubbedAccountGateway accountGateway = new();
        StopAccountTracking sut = new(accountGateway);

        Guid id = Guid.NewGuid();
        await sut.Execute(id);

        accountGateway.Calls.Should().Equal(id);
    }
}
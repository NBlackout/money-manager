namespace MoneyManager.Application.Write.Tests;

public class StopAccountTrackingTests
{
    [Fact]
    public async Task Should_stop_account_from_being_tracked()
    {
        InMemoryAccountRepository repository = new();
        StopAccountTracking sut = new(repository);

        AccountSnapshot existing = new(Guid.NewGuid(), "Bank", "Number", 86.50m, true);
        repository.Feed(existing);

        await sut.Execute(existing.Id);

        Account actual = await repository.GetById(existing.Id);
        actual.Snapshot.Should().Be(existing with { Tracked = false });
    }
}
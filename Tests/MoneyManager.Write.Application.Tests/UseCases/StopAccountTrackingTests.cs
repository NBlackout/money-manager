namespace MoneyManager.Write.Application.Tests.UseCases;

public class StopAccountTrackingTests
{
    private readonly InMemoryAccountRepository repository;
    private readonly StopAccountTracking sut;

    public StopAccountTrackingTests()
    {
        this.repository = new InMemoryAccountRepository();
        this.sut = new StopAccountTracking(this.repository);
    }

    [Fact]
    public async Task Should_stop_account_from_being_tracked()
    {
        Account existing = (AccountBuilder.For(Guid.NewGuid()) with { Tracked = true }).Build();
        this.repository.Feed(existing);

        await this.sut.Execute(existing.Id);

        Account actual = await this.repository.ById(existing.Id);
        actual.Snapshot.Should().Be(existing.Snapshot with { Tracked = false });
    }
}
namespace MoneyManager.Write.Application.Tests.UseCases;

public class ResumeAccountTrackingTests
{
    private readonly InMemoryAccountRepository repository;
    private readonly ResumeAccountTracking sut;

    public ResumeAccountTrackingTests()
    {
        this.repository = new InMemoryAccountRepository();
        this.sut = new ResumeAccountTracking(this.repository);
    }

    [Fact]
    public async Task Should_resume_account_tracking()
    {
        Account account = (AccountBuilder.For(Guid.NewGuid()) with { Tracked = false }).Build();
        this.repository.Feed(account);

        await this.sut.Execute(account.Id);

        Account actual = await this.repository.ById(account.Id);
        actual.Snapshot.Should().Be(account.Snapshot with { Tracked = true });
    }
}
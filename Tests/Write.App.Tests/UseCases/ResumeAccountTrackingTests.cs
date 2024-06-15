namespace Write.App.Tests.UseCases;

public class ResumeAccountTrackingTests
{
    private readonly InMemoryAccountRepository repository = new();
    private readonly ResumeAccountTracking sut;

    public ResumeAccountTrackingTests()
    {
        this.sut = new ResumeAccountTracking(this.repository);
    }

    [Theory, RandomData]
    public async Task Should_resume_account_tracking(AccountBuilder account)
    {
        this.repository.Feed((account with { Tracked = false }).Build());

        await this.sut.Execute(account.Id);

        Account actual = await this.repository.By(account.Id);
        actual.Snapshot.Should().Be(account.ToSnapshot() with { Tracked = true });
    }
}
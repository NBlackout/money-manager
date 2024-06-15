namespace Write.App.Tests.UseCases;

public class AssignAccountLabelTests
{
    private readonly InMemoryAccountRepository repository = new();
    private readonly AssignAccountLabel sut;

    public AssignAccountLabelTests()
    {
        this.sut = new AssignAccountLabel(this.repository);
    }

    [Fact]
    public async Task Should_assign_account_label()
    {
        Account account = (AccountBuilder.For(Guid.NewGuid()) with { Label = "Label to change" }).Build();
        this.repository.Feed(account);

        await this.sut.Execute(account.Id, "My account label");

        Account actual = await this.repository.ById(account.Id);
        actual.Snapshot.Should().Be(account.Snapshot with { Label = "My account label" });
    }
}
using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class RenameAccountTests
{
    private readonly InMemoryAccountRepository repository = new();
    private readonly RenameAccount sut;
    private readonly AccountSnapshot existingAccount = Any<AccountSnapshot>();

    public RenameAccountTests()
    {
        this.sut = new RenameAccount(this.repository);
        this.Feed(this.existingAccount);
    }

    [Theory]
    [RandomData]
    public async Task Renames_account(Label label) =>
        await this.Verify(label);

    private async Task Verify(Label label)
    {
        await this.sut.Execute(this.existingAccount.Id, label);

        Account actual = await this.repository.By(this.existingAccount.Id);
        actual.Snapshot.Should().Be(this.existingAccount with { Label = label.Value });
    }

    private void Feed(AccountSnapshot account) =>
        this.repository.Feed(account);
}
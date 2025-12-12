using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases;

public class RenameAccount(IAccountRepository repository)
{
    public async Task Execute(AccountId id, Label label)
    {
        Account account = await repository.By(id);
        account.Rename(label);
        await repository.Save(account);
    }
}
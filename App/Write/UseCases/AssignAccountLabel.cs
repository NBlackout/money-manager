using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases;

public class AssignAccountLabel(IAccountRepository repository)
{
    public async Task Execute(AccountId id, Label label)
    {
        Account account = await repository.By(id);
        account.AssignLabel(label);
        await repository.Save(account);
    }
}
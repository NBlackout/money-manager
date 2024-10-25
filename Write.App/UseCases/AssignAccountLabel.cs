namespace Write.App.UseCases;

public class AssignAccountLabel(IAccountRepository repository)
{
    public async Task Execute(AccountId id, string label)
    {
        Account account = await repository.By(id);
        account.AssignLabel(label);
        await repository.Save(account);
    }
}
namespace Write.App.UseCases;

public class StopAccountTracking(IAccountRepository repository)
{
    public async Task Execute(Guid id)
    {
        Account account = await repository.By(id);
        account.StopTracking();
        await repository.Save(account);
    }
}
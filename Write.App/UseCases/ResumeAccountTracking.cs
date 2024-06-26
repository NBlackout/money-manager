namespace Write.App.UseCases;

public class ResumeAccountTracking(IAccountRepository repository)
{
    public async Task Execute(Guid id)
    {
        Account account = await repository.By(id);
        account.ResumeTracking();
        await repository.Save(account);
    }
}
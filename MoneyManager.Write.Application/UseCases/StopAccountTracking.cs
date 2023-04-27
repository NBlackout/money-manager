namespace MoneyManager.Write.Application.UseCases;

public class StopAccountTracking
{
    private readonly IAccountRepository repository;

    public StopAccountTracking(IAccountRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id)
    {
        Account account = await this.repository.GetById(id);
        account.StopTracking();
        await this.repository.Save(account);
    }
}
namespace MoneyManager.Write.Application.UseCases;

public class ResumeAccountTracking
{
    private readonly IAccountRepository repository;

    public ResumeAccountTracking(IAccountRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id)
    {
        Account account = await this.repository.GetById(id);
        account.ResumeTracking();
        await this.repository.Save(account);
    }
}
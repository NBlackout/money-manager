namespace MoneyManager.Write.Application.UseCases;

public class AssignAccountLabel
{
    private readonly IAccountRepository repository;

    public AssignAccountLabel(IAccountRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, string label)
    {
        Account account = await this.repository.GetById(id);
        account.AssignLabel(label);
        await this.repository.Save(account);
    }
}
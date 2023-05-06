namespace MoneyManager.Write.Application.UseCases;

public class AssignBankName
{
    private readonly IBankRepository repository;

    public AssignBankName(IBankRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, string name)
    {
        Bank bank = await this.repository.ById(id);
        bank.AssignName(name);
        await this.repository.Save(bank);
    }
}
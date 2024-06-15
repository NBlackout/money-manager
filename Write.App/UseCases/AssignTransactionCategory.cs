namespace Write.App.UseCases;

public class AssignTransactionCategory
{
    private readonly ITransactionRepository repository;

    public AssignTransactionCategory(ITransactionRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, Guid categoryId)
    {
        Transaction transaction = await this.repository.By(id);
        transaction.AssignCategory(categoryId);
        await this.repository.Save(transaction);
    }
}
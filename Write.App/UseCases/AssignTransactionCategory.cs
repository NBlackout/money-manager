namespace Write.App.UseCases;

public class AssignTransactionCategory(ITransactionRepository repository)
{
    public async Task Execute(Guid id, Guid categoryId)
    {
        Transaction transaction = await repository.By(id);
        transaction.AssignCategory(categoryId);
        await repository.Save(transaction);
    }
}
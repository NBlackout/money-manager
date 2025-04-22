namespace Write.App.UseCases;

public class DeleteCategory(ICategoryRepository repository, ITransactionRepository transactionRepository)
{
    public async Task Execute(CategoryId id)
    {
        Transaction[] transactions = await transactionRepository.By(id);
        UnassignCategoryOf(transactions);
        await this.Save(id, transactions);
    }

    private static void UnassignCategoryOf(Transaction[] transactions)
    {
        foreach (Transaction transaction in transactions)
            transaction.UnassignCategory();
    }

    private async Task Save(CategoryId id, Transaction[] transactions)
    {
        await repository.Delete(id);
        await transactionRepository.Save(transactions);
    }
}
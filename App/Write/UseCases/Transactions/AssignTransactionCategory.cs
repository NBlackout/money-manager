using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.Ports;

namespace App.Write.UseCases.Transactions;

public class AssignTransactionCategory(ITransactionRepository repository)
{
    public async Task Execute(TransactionId id, CategoryId categoryId)
    {
        Transaction transaction = await repository.By(id);
        transaction.AssignCategory(categoryId);
        await repository.Save(transaction);
    }
}
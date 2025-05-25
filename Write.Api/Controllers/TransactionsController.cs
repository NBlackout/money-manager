using Write.App.Model.Categories;
using Write.App.Model.Transactions;

namespace Write.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController(AssignTransactionCategory assignTransactionCategory) : ControllerBase
{
    [HttpPut("{id:guid}/category")]
    public async Task AssignCategory(Guid id, TransactionCategoryDto dto) =>
        await assignTransactionCategory.Execute(new TransactionId(id), new CategoryId(dto.CategoryId));
}
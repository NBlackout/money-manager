namespace MoneyManager.Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly AssignTransactionCategory assignTransactionCategory;

    public TransactionsController(AssignTransactionCategory assignTransactionCategory)
    {
        this.assignTransactionCategory = assignTransactionCategory;
    }

    [HttpPut]
    [Route("{id:guid}/category")]
    public async Task AssignCategory(Guid id, TransactionCategoryDto dto) =>
        await this.assignTransactionCategory.Execute(id, dto.CategoryId);
}
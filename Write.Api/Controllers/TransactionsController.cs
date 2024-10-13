namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(AssignTransactionCategory assignTransactionCategory) : ControllerBase
{
    [HttpPut("{id:guid}/category")]
    public async Task AssignCategory(Guid id, TransactionCategoryDto dto) =>
        await assignTransactionCategory.Execute(id, dto.CategoryId);
}
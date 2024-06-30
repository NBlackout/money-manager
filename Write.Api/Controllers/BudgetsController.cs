namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetsController(DefineBudget defineBudget) : ControllerBase
{
    [HttpPost]
    public async Task Define(BudgetDto dto) =>
        await defineBudget.Execute(dto.Id, dto.Name, dto.Amount);
}
using Write.App.Model.Budgets;
using Write.App.Model.ValueObjects;

namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetsController(DefineBudget defineBudget) : ControllerBase
{
    [HttpPost]
    public async Task Define(BudgetDto dto) =>
        await defineBudget.Execute(new BudgetId(dto.Id), new Label(dto.Name), new Amount(dto.Amount), dto.BeginDate);
}
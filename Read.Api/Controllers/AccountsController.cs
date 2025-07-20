namespace Read.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(AccountSummaries accountSummaries, AccountDetails accountDetails, TransactionsOfMonth transactionsOfMonth) : ControllerBase
{
    [HttpGet]
    public async Task<AccountSummaryPresentation[]> Summaries() =>
        await accountSummaries.Execute();

    [HttpGet("{id:guid}")]
    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        await accountDetails.Execute(id);

    [HttpGet("{id:guid}/transactions")]
    public async Task<TransactionSummaryPresentation[]> Details(Guid id, [FromQuery] int year, [FromQuery] int month) =>
        await transactionsOfMonth.Execute(id, year, month);
}
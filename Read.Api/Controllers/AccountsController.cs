namespace Read.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountSummaries accountSummaries;
    private readonly AccountDetails accountDetails;
    private readonly TransactionsOfMonth transactionsOfMonth;

    public AccountsController(AccountSummaries accountSummaries, AccountDetails accountDetails,
        TransactionsOfMonth transactionsOfMonth)
    {
        this.accountSummaries = accountSummaries;
        this.accountDetails = accountDetails;
        this.transactionsOfMonth = transactionsOfMonth;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Summaries() =>
        await this.accountSummaries.Execute();

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        await this.accountDetails.Execute(id);

    [HttpGet]
    [Route("{id:guid}/transactions")]
    public async Task<IReadOnlyCollection<TransactionSummaryPresentation>> Details(Guid id, [FromQuery] int year,
        [FromQuery] int month) =>
        await this.transactionsOfMonth.Execute(id, year, month);
}
namespace MoneyManager.Read.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountSummaries accountSummaries;
    private readonly AccountDetails accountDetails;

    public AccountsController(AccountSummaries accountSummaries, AccountDetails accountDetails)
    {
        this.accountSummaries = accountSummaries;
        this.accountDetails = accountDetails;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Summaries() =>
        await this.accountSummaries.Execute();

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        await this.accountDetails.Execute(id);
}
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Read.AccountSummaries;

namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly GetAccountSummaries getAccountSummaries;

    public AccountsController(GetAccountSummaries getAccountSummaries)
    {
        this.getAccountSummaries = getAccountSummaries;
    }
    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummary>> Get()
    {
        return await getAccountSummaries.Handle();
    }
}
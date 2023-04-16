using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Read.UseCases.AccountSummaries;

namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly GetAccountSummaries getAccountSummaries;

    public AccountsController(GetAccountSummaries getAccountSummaries)
    {
        this.getAccountSummaries = getAccountSummaries;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummary>> Get() =>
        await this.getAccountSummaries.Execute();
}
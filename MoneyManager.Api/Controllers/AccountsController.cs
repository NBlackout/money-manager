using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Application.Write.UseCases;

namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly GetAccountSummaries getAccountSummaries;
    private readonly ImportTransactions importTransactions;

    public AccountsController(GetAccountSummaries getAccountSummaries, ImportTransactions importTransactions)
    {
        this.getAccountSummaries = getAccountSummaries;
        this.importTransactions = importTransactions;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummary>> Get() =>
        await this.getAccountSummaries.Execute();

    [HttpPost]
    public async Task Upload([FromForm] IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();
        await this.importTransactions.Execute(stream);
    }
}
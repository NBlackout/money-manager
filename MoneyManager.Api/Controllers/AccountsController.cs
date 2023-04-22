using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Application.Write.UseCases;

namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly GetAccountSummaries getAccountSummaries;
    private readonly ImportBankStatement importBankStatement;

    public AccountsController(GetAccountSummaries getAccountSummaries, ImportBankStatement importBankStatement)
    {
        this.getAccountSummaries = getAccountSummaries;
        this.importBankStatement = importBankStatement;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummary>> Get() =>
        await this.getAccountSummaries.Execute();

    [HttpPost]
    public async Task Upload([FromForm] IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();
        await this.importBankStatement.Execute(stream);
    }
}
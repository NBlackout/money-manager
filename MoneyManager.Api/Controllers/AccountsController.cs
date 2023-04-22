using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Read.UseCases;
using MoneyManager.Application.Write.UseCases;

namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly GetAccountSummaries getAccountSummaries;
    private readonly ImportBankStatement importBankStatement;
    private readonly StopAccountTracking stopAccountTracking;

    public AccountsController(GetAccountSummaries getAccountSummaries, ImportBankStatement importBankStatement,
        StopAccountTracking stopAccountTracking)
    {
        this.getAccountSummaries = getAccountSummaries;
        this.importBankStatement = importBankStatement;
        this.stopAccountTracking = stopAccountTracking;
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

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task StopTracking(Guid id) =>
        await this.stopAccountTracking.Execute(id);
}
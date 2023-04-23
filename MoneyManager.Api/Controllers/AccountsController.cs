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
    private readonly ResumeAccountTracking resumeAccountTracking;
    private readonly StopAccountTracking stopAccountTracking;

    public AccountsController(GetAccountSummaries getAccountSummaries, ImportBankStatement importBankStatement,
        ResumeAccountTracking resumeAccountTracking, StopAccountTracking stopAccountTracking)
    {
        this.getAccountSummaries = getAccountSummaries;
        this.importBankStatement = importBankStatement;
        this.resumeAccountTracking = resumeAccountTracking;
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

    [HttpPut]
    [Route("{id:guid}/tracking")]
    public async Task ChangeTrackingStatus(Guid id, ChangeTrackingStatusDto dto)
    {
        if (dto.Enabled)
            await this.resumeAccountTracking.Execute(id);
        else
            await this.stopAccountTracking.Execute(id);
    }
}
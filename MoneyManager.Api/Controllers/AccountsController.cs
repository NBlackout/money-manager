﻿namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountSummaries accountSummaries;
    private readonly AccountDetails accountDetails;
    private readonly ImportBankStatement importBankStatement;
    private readonly ResumeAccountTracking resumeAccountTracking;
    private readonly StopAccountTracking stopAccountTracking;
    private readonly AssignAccountLabel assignAccountLabel;

    public AccountsController(AccountSummaries accountSummaries, AccountDetails accountDetails,
        ImportBankStatement importBankStatement,
        ResumeAccountTracking resumeAccountTracking, StopAccountTracking stopAccountTracking,
        AssignAccountLabel assignAccountLabel)
    {
        this.accountSummaries = accountSummaries;
        this.accountDetails = accountDetails;
        this.importBankStatement = importBankStatement;
        this.resumeAccountTracking = resumeAccountTracking;
        this.stopAccountTracking = stopAccountTracking;
        this.assignAccountLabel = assignAccountLabel;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Summaries() =>
        await this.accountSummaries.Execute();

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        await this.accountDetails.Execute(id);

    [HttpPost]
    public async Task Upload([FromForm] IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();
        await this.importBankStatement.Execute(stream);
    }

    [HttpPut]
    [Route("{id:guid}/label")]
    public async Task AssignLabel(Guid id, AccountLabelDto dto) =>
        await this.assignAccountLabel.Execute(id, dto.Label);

    [HttpPut]
    [Route("{id:guid}/tracking")]
    public async Task ChangeTrackingStatus(Guid id, TrackingStatusDto dto)
    {
        if (dto.Enabled)
            await this.resumeAccountTracking.Execute(id);
        else
            await this.stopAccountTracking.Execute(id);
    }
}
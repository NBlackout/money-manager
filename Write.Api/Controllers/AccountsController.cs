namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ImportBankStatement importBankStatement;
    private readonly ResumeAccountTracking resumeAccountTracking;
    private readonly StopAccountTracking stopAccountTracking;
    private readonly AssignAccountLabel assignAccountLabel;

    public AccountsController(ImportBankStatement importBankStatement, ResumeAccountTracking resumeAccountTracking,
        StopAccountTracking stopAccountTracking, AssignAccountLabel assignAccountLabel)
    {
        this.importBankStatement = importBankStatement;
        this.resumeAccountTracking = resumeAccountTracking;
        this.stopAccountTracking = stopAccountTracking;
        this.assignAccountLabel = assignAccountLabel;
    }

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
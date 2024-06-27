namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(
    ImportBankStatement importBankStatement,
    ResumeAccountTracking resumeAccountTracking,
    StopAccountTracking stopAccountTracking,
    AssignAccountLabel assignAccountLabel)
    : ControllerBase
{
    [HttpPost]
    public async Task Upload([FromForm] IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();
        await importBankStatement.Execute(file.FileName, stream);
    }

    [HttpPut]
    [Route("{id:guid}/label")]
    public async Task AssignLabel(Guid id, AccountLabelDto dto) =>
        await assignAccountLabel.Execute(id, dto.Label);

    [HttpPut]
    [Route("{id:guid}/tracking")]
    public async Task ChangeTrackingStatus(Guid id, TrackingStatusDto dto)
    {
        if (dto.Enabled)
            await resumeAccountTracking.Execute(id);
        else
            await stopAccountTracking.Execute(id);
    }
}
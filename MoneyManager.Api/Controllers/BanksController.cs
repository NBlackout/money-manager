namespace MoneyManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BanksController : ControllerBase
{
    private readonly AssignBankName assignBankName;

    public BanksController(AssignBankName assignBankName)
    {
        this.assignBankName = assignBankName;
    }

    [HttpPut]
    [Route("{id:guid}/name")]
    public async Task AssignName(Guid id, BankNameDto dto) =>
        await this.assignBankName.Execute(id, dto.Name);
}
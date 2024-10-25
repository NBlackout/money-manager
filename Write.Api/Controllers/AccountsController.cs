using Microsoft.AspNetCore.Http;
using Write.App.Model.Accounts;
using Write.App.Model.ValueObjects;

namespace Write.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(ImportBankStatement importBankStatement, AssignAccountLabel assignAccountLabel)
    : ControllerBase
{
    [HttpPost]
    public async Task Upload([FromForm] IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();
        await importBankStatement.Execute(file.FileName, stream);
    }

    [HttpPut("{id:guid}/label")]
    public async Task AssignLabel(Guid id, AccountLabelDto dto) =>
        await assignAccountLabel.Execute(new AccountId(id), new Label(dto.Label));
}
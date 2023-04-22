using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyManager.Client.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Client.Application.Write.UseCases.BankStatement;

namespace MoneyManager.Client.Pages;

public partial class Accounts : ComponentBase
{
    private const int OneMegaByte = 1024000;

    private IReadOnlyCollection<AccountSummary>? accounts;
    private string? uploadResult;

    [Inject] private GetAccountSummaries GetAccountSummaries { get; set; } = null!;
    [Inject] private UploadBankStatement UploadBankStatement { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await this.LoadAccounts();
    }

    private async Task UploadBankStatementFile(InputFileChangeEventArgs args)
    {
        try
        {
            await this.Upload(args.File);

            this.uploadResult = "Bank statement successfully imported";
            await this.LoadAccounts();
        }
        catch (Exception e)
        {
            this.uploadResult = e.Message;
        }
    }

    private async Task LoadAccounts() =>
        this.accounts = await this.GetAccountSummaries.Execute();

    private async Task Upload(IBrowserFile file)
    {
        string fileName = file.Name;
        string contentType = file.ContentType;
        Stream stream = file.OpenReadStream(OneMegaByte);

        await this.UploadBankStatement.Execute(fileName, contentType, stream);
    }
}
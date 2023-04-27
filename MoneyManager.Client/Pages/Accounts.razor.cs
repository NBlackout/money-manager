using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;
using MoneyManager.Client.Write.Application.UseCases;

namespace MoneyManager.Client.Pages;

public partial class Accounts : ComponentBase
{
    private const int OneMegaByte = 1024000;

    private IReadOnlyCollection<AccountSummary>? accounts;
    private string? uploadResult;

    [Inject] private GetAccountSummaries GetAccountSummaries { get; set; } = null!;
    [Inject] private UploadBankStatement UploadBankStatement { get; set; } = null!;
    [Inject] private StopAccountTracking StopAccountTracking { get; set; } = null!;
    [Inject] private ResumeAccountTracking ResumeAccountTracking { get; set; } = null!;

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

    private async Task StopTracking(Guid id)
    {
        await this.StopAccountTracking.Execute(id);
        this.Patch(id, false);
    }

    private async Task ResumeTracking(Guid id)
    {
        await this.ResumeAccountTracking.Execute(id);
        this.Patch(id, true);
    }

    private void Patch(Guid id, bool tracked)
    {
        List<AccountSummary> updatedAccounts = this.accounts!.ToList();
        AccountSummary account = updatedAccounts.Single(a => a.Id == id);
        int index = updatedAccounts.IndexOf(account);
        updatedAccounts[index] = account with { Tracked = tracked };

        this.accounts = updatedAccounts;
    }
}
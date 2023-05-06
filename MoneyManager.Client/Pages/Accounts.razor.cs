using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;

namespace MoneyManager.Client.Pages;

public partial class Accounts : ComponentBase
{
    private const int OneMegaByte = 1024000;

    private IReadOnlyCollection<AccountSummaryPresentation>? accounts;
    private AccountSummaryPresentation? accountBeingEdited;
    private string? uploadResult;
    private ElementReference labelInput;
    private ElementReference bankNameInput;

    [Inject] private AccountSummaries AccountSummaries { get; set; } = null!;
    [Inject] private UploadBankStatement UploadBankStatement { get; set; } = null!;
    [Inject] private StopAccountTracking StopAccountTracking { get; set; } = null!;
    [Inject] private ResumeAccountTracking ResumeAccountTracking { get; set; } = null!;
    [Inject] private AssignAccountLabel AssignAccountLabel { get; set; } = null!;
    [Inject] private AssignBankName AssignBankName { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        await this.LoadAccounts();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.accountBeingEdited != null)
            await this.labelInput.FocusAsync();
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
        this.accounts = await this.AccountSummaries.Execute();

    private bool IsEditing(AccountSummaryPresentation account) =>
        this.accountBeingEdited == account;

    private void ToggleEditMode(AccountSummaryPresentation account) =>
        this.accountBeingEdited = account;

    private void ExitEditMode() =>
        this.accountBeingEdited = null;

    private async Task Upload(IBrowserFile file)
    {
        string fileName = file.Name;
        string contentType = file.ContentType;
        Stream stream = file.OpenReadStream(OneMegaByte);

        await this.UploadBankStatement.Execute(fileName, contentType, stream);
    }

    private async Task BankNameChanged(ChangeEventArgs args)
    {
        string newBankName = (string)args.Value!;
        await this.AssignBankName.Execute(this.accountBeingEdited!.BankId, newBankName);

        foreach (AccountSummaryPresentation bankAccount in this.accounts!.Where(a => a.BankId == this.accountBeingEdited.BankId))
            this.Patch(bankAccount with { BankName = newBankName });
        this.ExitEditMode();
    }

    private async Task LabelChanged(ChangeEventArgs args)
    {
        string newLabel = (string)args.Value!;
        await this.AssignAccountLabel.Execute(this.accountBeingEdited!.Id, newLabel);

        this.Patch(this.accountBeingEdited with { Label = newLabel });
        this.ExitEditMode();
    }

    private async Task StopTracking(AccountSummaryPresentation account)
    {
        await this.StopAccountTracking.Execute(account.Id);
        this.Patch(account with { Tracked = false });
    }

    private async Task ResumeTracking(AccountSummaryPresentation account)
    {
        await this.ResumeAccountTracking.Execute(account.Id);
        this.Patch(account with { Tracked = true });
    }

    private void Patch(AccountSummaryPresentation updatedAccount)
    {
        List<AccountSummaryPresentation> updatedAccounts = this.accounts!.ToList();
        AccountSummaryPresentation account = updatedAccounts.Single(a => a.Id == updatedAccount.Id);
        int index = updatedAccounts.IndexOf(account);
        updatedAccounts[index] = updatedAccount;

        this.accounts = updatedAccounts;
    }
}
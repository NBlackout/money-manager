namespace MoneyManager.Client.Pages;

public partial class Accounts : ComponentBase
{
    private IReadOnlyCollection<AccountSummaryPresentation>? accounts;
    private AccountSummaryPresentation? accountBeingEdited;
    private ElementReference bankNameInput;

    [Inject] private AccountSummaries AccountSummaries { get; set; } = null!;
    [Inject] private UploadBankStatement UploadBankStatement { get; set; } = null!;
    [Inject] private StopAccountTracking StopAccountTracking { get; set; } = null!;
    [Inject] private ResumeAccountTracking ResumeAccountTracking { get; set; } = null!;
    [Inject] private AssignBankName AssignBankName { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    [Parameter] public Guid? SelectedId { get; set; }

    protected override async Task OnInitializedAsync() =>
        await this.LoadAccounts();

    private async Task LoadAccounts() =>
        this.accounts = await this.AccountSummaries.Execute();

    private bool IsEditing(AccountSummaryPresentation account) =>
        this.accountBeingEdited == account;

    private void ToggleEditMode(AccountSummaryPresentation account)
    {
        this.accountBeingEdited = account;
    }

    private void ExitEditMode() =>
        this.accountBeingEdited = null;

    private void NavigateToAccount(Guid accountId)
    {
        if (this.accountBeingEdited == null)
            this.NavigationManager.NavigateTo($"accounts/{accountId}");
    }

    private async Task BankNameChanged(ChangeEventArgs args)
    {
        string newBankName = (string)args.Value!;
        await this.AssignBankName.Execute(this.accountBeingEdited!.BankId, newBankName);

        foreach (AccountSummaryPresentation bankAccount in this.accounts!.Where(a =>
                     a.BankId == this.accountBeingEdited.BankId))
            this.Patch(bankAccount with { BankName = newBankName });
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

    private void ShowDetails(Guid accountId) =>
        this.SelectedId = accountId;
}
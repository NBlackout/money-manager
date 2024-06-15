namespace Client.Pages;

public partial class Accounts : ComponentBase
{
    private IReadOnlyCollection<AccountSummaryPresentation>? accounts;

    [Inject] private AccountSummaries AccountSummaries { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public Guid? SelectedId { get; set; }

    protected override async Task OnInitializedAsync() =>
        this.accounts = await this.AccountSummaries.Execute();

    private void ShowDetails(Guid accountId) =>
        this.NavigationManager.NavigateTo($"accounts/{accountId}");
}
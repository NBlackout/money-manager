namespace MoneyManager.Client.Pages;

public partial class Account : ComponentBase
{
    private AccountDetailsPresentation? account;

    [Parameter] public Guid Id { get; set; }
    [Inject] private AccountDetails AccountDetails { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        this.account = await this.AccountDetails.Execute(this.Id);
}
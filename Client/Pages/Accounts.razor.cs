﻿namespace Client.Pages;

public partial class Accounts : ComponentBase
{
    private AccountSummaryPresentation[]? accounts;
    private BudgetSummaryPresentation[]? budgets;

    [Inject] private AccountSummaries AccountSummaries { get; set; } = null!;
    [Inject] private BudgetSummaries BudgetSummaries { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public Guid? SelectedId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.accounts = await this.AccountSummaries.Execute();
        this.budgets = await this.BudgetSummaries.Execute();
    }

    private void ShowDetails(Guid accountId) =>
        this.NavigationManager.NavigateTo($"accounts/{accountId}");
}
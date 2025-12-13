using App.Read.Ports;
using App.Read.UseCases;

namespace Client.Components;

public partial class ExpectedTransactions : ComponentBase
{
    private ExpectedTransactionSummaryPresentation[]? transactions;

    [Inject] private ExpectedTransactionSummaries ExpectedTransactionSummaries { get; set; } = null!;

    [Parameter] public DateOnly Month { get; set; }

    private decimal Total => this.transactions!.Sum(t => t.Amount);

    protected override async Task OnParametersSetAsync() =>
        this.transactions = await this.ExpectedTransactionSummaries.Execute(this.Month.Year, this.Month.Month);
}
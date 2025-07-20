using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Dashboard
{
    [Inject] public SlidingBalances SlidingBalances { get; set; } = null!;
    private SlidingBalancesPresentation? slidingBalances;

    protected override async Task OnInitializedAsync() =>
        this.slidingBalances = await this.SlidingBalances.Execute();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.slidingBalances != null && this.slidingBalances.SlidingBalances.Length != 0)
        {
            HighchartsRenderer renderer = new(
                new Highcharts
                {
                    Title = new Title { Text = "Sliding balances" },
                    XAxis = [new XAxis { Categories = CategoriesBy(this.slidingBalances!) }],
                    YAxis = [new YAxis { Title = new YAxisTitle { Text = "Balance" } }],
                    Credits = new Credits { Enabled = false },
                    Series = SeriesBy(this.slidingBalances!)
                }
            );

            await this.JsRuntime.InvokeVoidAsync("window.renderChart", "dashboard-chart", renderer.GetJsonOptionsForBlazor());
        }
    }

    private static List<string> CategoriesBy(SlidingBalancesPresentation presentation) =>
        presentation.SlidingBalances.Select(b => b.BalanceDate.ToShortDateString()).ToList();

    private static List<Series> SeriesBy(SlidingBalancesPresentation presentation) =>
        presentation
            .SlidingBalances
            .SelectMany(d => d.AccountBalances.Select(b => b.AccountLabel))
            .Distinct()
            .Select(a => SeriesBy(presentation, a))
            .ToList<Series>();

    private static LineSeries SeriesBy(SlidingBalancesPresentation presentation, string accountLabel) =>
        new()
        {
            Name = accountLabel,
            Data = presentation.SlidingBalances.Select(b => AreaSeriesDataBy(accountLabel, b)).ToList(),
            Tooltip = new LineSeriesTooltip { ValueSuffix = " €" }
        };

    private static LineSeriesData AreaSeriesDataBy(string accountLabel, SlidingBalancePresentation date) =>
        new() { Y = decimal.ToDouble(date.AccountBalances.Single(b => b.AccountLabel == accountLabel).Balance) };
}